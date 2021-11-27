using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public PlayerStatsComponent playerStatsComponent;
    // Constants for movement speed and jump
    [SerializeField]
    private float moveSpeedMax = 7.0f;
    private float moveSpeed = 0.0f;
    private float storedMoveSpeed;
    [SerializeField]
    private float sprintMaxSpeed = 20.0f;
    [SerializeField]
    private int maxStamina = 80;
    [SerializeField]
    public int currentStamina;
    [SerializeField]
    private int staminaBurnRate = 2;
    [SerializeField]
    private int staminaRecoveryRate = 1;
    private float staminaTimerTime = 1.0f;
    private float staminaTimer;
    // acceleration for speed up of character going from 0 velocity to full velocity.
    [SerializeField]
    private float acceleration = 15.0f;
    [SerializeField]
    private float deceleration = 15.0f;

    // get hit timer to give the player a little room to stay safe.
    private float getHitTimerMax = 0.50f;
    private float getHitTimer = 0;

    private bool canMove;
    private bool isAlive;
    private bool canInput;
    // Setup attributes and components needed for movement of player.
    private Rigidbody2D rb;
    Vector2 movement;
    Vector2 storePreviousMovement;

    private Animator anim;
    private Canvas canvas;
    private StaminaBar staminaBar;
    private GameObject stam;
    private GameObject Pick;
    private StaminaBar PickUpBar;

    //Attack Variables
    public int attackDamage;
    public int baseAttackDamage = 20;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public Transform attackPointUp;
    public Transform attackPointDown;
    private Transform previousAttackPoint;
    private Vector2 previousRangedAttackDirection;
    public Transform currentAttackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;

    public AudioClip attackSwingSound;
    public AudioClip hitSound;
    public AudioClip fireBallSound;
    public AudioClip potionPickUpSound;
    public AudioClip weaponPickUpSound;
    private AudioSource audioSource;

    // Pick up Item Variables
    public bool canPickUp = false;
    public GameObject itemToPickUp;
    private int[] swordPickUpDamages = {40, 60, 80, 100, 120, 150};
    private bool rangedAttackOn = false;
    public GameObject[] rangedObjects;
    private GameObject rangedObject;
    private Vector2 rangedAttackDirection;
    public GameObject bombObject;

    private float pickupMaxTime = 30.0f;
    private float currentPickupTime;
    private bool isMagic;

    public GameObject tempGenerator;
    private CanvasParts canvasParts;

    // Initialize variables for the player.
    void Start(){
        canvasParts = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasParts>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        storePreviousMovement = new Vector2(0,0);
        currentAttackPoint = attackPointDown;
        currentStamina = maxStamina;
        stam = canvasParts.GetStaminaBar();
        Pick = canvasParts.GetPickUpBar();
        staminaBar = stam.GetComponent<StaminaBar>();
        PickUpBar = Pick.GetComponent<StaminaBar>();
        canMove = true;
        isAlive = true;
        canInput = true;
        audioSource = GetComponent<AudioSource>();
        setAttackDamage(baseAttackDamage);
        staminaBar.setStamina(maxStamina);
        staminaBar.SetMaxValue(maxStamina);
        PickUpBar.SetMaxValue((int) pickupMaxTime);
        PickUpBar.setStamina(0);
        playerStatsComponent = GameObject.Find("PlayerStats").GetComponent<PlayerStatsComponent>();
    } 

    // Update is called once per frame, Contains primary input from player.
    void Update()
    {  

        if (isAlive && canInput){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            anim.SetFloat("Speed", movement.sqrMagnitude);

            float xdirection = movement.x;
            float ydirection = movement.y;
            // simple movement information
            if (canInput){
            if (movement.magnitude > .02){
                storePreviousMovement = new Vector2(movement.x, movement.y);
                if (Mathf.Abs(xdirection) > Mathf.Abs(ydirection)){
                    anim.SetFloat("Xdirection", xdirection);
                    anim.SetFloat("Ydirection", 0);
                }
                else {
                    anim.SetFloat("Xdirection", 0);
                    anim.SetFloat("Ydirection", ydirection);
                    }
                }
            
                if (Input.GetKeyDown(KeyCode.Space)){
                    // storePreviousMovement = new Vector2(movement.x, movement.y);
                    canMove = false;
                    canInput = false;
                    Attack();
                }

                if (Input.GetKeyDown("e") && canPickUp){
                    // pick up items.
                    pickUpItem();
                }
                if (Input.GetKeyDown("b"))
                {
                    if (playerStatsComponent.getNumBomb() >= 1) 
                    {
                        playerStatsComponent.modifyBombs(-1);
                        GameObject newBomb = Instantiate(bombObject, currentAttackPoint.position, Quaternion.identity);
                        newBomb.GetComponent<Bomb_Pickup>();
                    }
                    else
                    {
                        Debug.Log("Player does not have any bombs to detonate.");
                    }
                }
            }
        }
    }

    /**
    Main FixedUpdate Method for movement, includes sprinting mechanic and stamina bar.
    */
    void FixedUpdate(){
        if (getHitTimer > 0){
            getHitTimer -= Time.fixedDeltaTime;
        }
        if (currentPickupTime > 0){
            currentPickupTime -= Time.fixedDeltaTime;
            if (currentPickupTime <= 0){
                rangedAttackOn = false;
            }
            PickUpBar.setStamina((int) currentPickupTime);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attacks")){
            moveSpeed = 0;
            canMove = false;
        }
        else if (isAlive){
            canMove = true;
        }
        if (canMove){
        if (Mathf.Abs(movement.x) > 0.02 || Mathf.Abs(movement.y) > 0.02){
            // if running and change direction quickly reset movespeed. (have to stop instantaniously to turn around.) may have to implement a minor timer to adjust for non instantanious transitions. 
            if ((storePreviousMovement[0] == -movement.x && storePreviousMovement[1] == -movement.y && moveSpeed > 0)){
                moveSpeed = 0;
            }
            storePreviousMovement = new Vector2(movement.x, movement.y);
            // Start sprint
            if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0){
                // MaxSpeed/Length of movement to normalize the speed velocity such that is the same for all directions including diagonal.
                if (moveSpeed < sprintMaxSpeed / Mathf.Sqrt(movement.x * movement.x + movement.y * movement.y)){
                    moveSpeed += acceleration * Time.fixedDeltaTime;
                }
                else{
                    moveSpeed = moveSpeedMax;
                }
                // drain stamina and start timer.
                currentStamina -= staminaBurnRate; 
                if (currentStamina <= maxStamina){
                    staminaTimer = staminaTimerTime;
                }
            }
            else {
                // non sprinting movement. accelerate from rest and decelerate to max speed.
                if (moveSpeed < moveSpeedMax){
                    moveSpeed += acceleration * Time.fixedDeltaTime;
                }
                else {
                    moveSpeed -= (deceleration / 2) * Time.fixedDeltaTime;
                }
            }
            // move the sprite character with position and movement speed based on previous inputs. normalized speed for diagonal movement.
            if (canMove){
            rb.MovePosition(rb.position + movement * (moveSpeed / Mathf.Sqrt(movement.x * movement.x + movement.y * movement.y)) * Time.fixedDeltaTime);
            }
        }

        else if (Mathf.Abs(movement.x) <=.02 && Mathf.Abs(movement.y) <= .02){
        //if player stops inputting. or input is too low (deadZone)
            if (moveSpeed - deceleration * Time.fixedDeltaTime >= 0){
                moveSpeed -= deceleration * Time.fixedDeltaTime;
                if (canMove){
                rb.MovePosition(rb.position + storePreviousMovement * (moveSpeed / Mathf.Sqrt(storePreviousMovement[0] * storePreviousMovement[0] + storePreviousMovement[1] * storePreviousMovement[1])) * Time.fixedDeltaTime);
                }
            }
        }
        // update Stamina bar
        staminaBar.setStamina(currentStamina);
        if (staminaTimer > 0){
            staminaTimer -= Time.fixedDeltaTime;
        }
        else{
            if (currentStamina < maxStamina){
                currentStamina += staminaRecoveryRate;
            }
        }
        }
    }
    

    // for if you hit an object and are still running need to reduce movement speed so that it isnt max if never let go controls.
    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.tag != "Wall"){
            if (col.gameObject.tag != "Coins"){
                moveSpeed = 0;
            }
        }
        moveSpeed = storedMoveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        storedMoveSpeed = moveSpeed;
        if (other.gameObject.tag != "Coins"){
            moveSpeed = 0;
        }    
    }

    // Make the player attack.
    void Attack(){
        float x = storePreviousMovement[0];
        float y = storePreviousMovement[1];
        // anim.SetFloat("Xdirection", storePreviousMovement[0]);
        // anim.SetFloat("Ydirection", storePreviousMovement[1]);
        //Debug.Log("x: " +  x + "y: " +y);
        anim.SetFloat("Xdirection", x);
        anim.SetFloat("Ydirection", y);
        anim.SetTrigger("attack");
        // set which attack point to use during attack animation.
        // this is right quadrant.
        if (x > 0 && y < .5 && y >-.5){
            currentAttackPoint = attackPointRight;
            rangedAttackDirection = Vector2.right;
        }
        else if (x == 1 && y == 1){
            currentAttackPoint = attackPointUp;
            rangedAttackDirection = Vector2.up;
        }
        else if ( x == -1 && y == 1){
            currentAttackPoint = attackPointUp;
            rangedAttackDirection = Vector2.up;
        }
        else if (x == -1 && y == -1){
            currentAttackPoint = attackPointDown;
            rangedAttackDirection = Vector2.down;
        }
        else if (x == 1 && y == -1){
            currentAttackPoint = attackPointDown;
            rangedAttackDirection = Vector2.down;
        }
        else if (x == 0 && y == 0){
            currentAttackPoint = previousAttackPoint;
            rangedAttackDirection = previousRangedAttackDirection;
        }
        // this is left quadrant
        else if (x < 0 && y <= .5 && y >= -.5){
            currentAttackPoint = attackPointLeft;
            rangedAttackDirection = Vector2.left;
        }
        // bottom Quadrant
        else if (y < 0 && x <= .5 && x >= -.5){
            currentAttackPoint = attackPointDown;
            rangedAttackDirection = Vector2.down;
        }
        // top Quadrant
        else if (y >= 0 && x <.5 && x > -.5) {
            currentAttackPoint = attackPointUp;
            rangedAttackDirection = Vector2.up;
        }
        previousAttackPoint = currentAttackPoint;
        previousRangedAttackDirection = rangedAttackDirection;
    }

    void rangedAttack(){
        GameObject newArrow = Instantiate(rangedObject, currentAttackPoint.position, Quaternion.identity);
        if (isMagic){
            newArrow.GetComponent<FireBall>().setDirection(rangedAttackDirection);
            audioSource.PlayOneShot(fireBallSound);
        }
        else {
        newArrow.GetComponent<Arrow>().setDirection(rangedAttackDirection);
        }
    }

    void attackSwingAudio(){
        audioSource.PlayOneShot(attackSwingSound);
    }

    // call the actual part of the animation where damage occurs.
    void attackAnimation(){
        if (rangedAttackOn){
            rangedAttack();
        }
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(currentAttackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in enemiesHit){
            if (enemy.gameObject.tag == "Enemy"){
            enemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage);
            }
        }
    }

    public void playerDie(){
        isAlive = false;
        canMove = false;
        moveSpeed = 0;
        anim.SetFloat("Xdirection", storePreviousMovement[0]);
        anim.SetFloat("Ydirection", storePreviousMovement[1]);
        anim.SetBool("isDead", true);

        Invoke("Respawn", 3);
    }

    private void Respawn()
    {
        //GameObject generator = GameObject.Find("RoomsFirstDungeonGenerator");
        //generator.SendMessage("GenerateDungeon");
        //generator.SendMessage("Respawn");
        anim.SetBool("isDead", false);
        this.isAlive = true;
        this.canMove = true;
        this.playerStatsComponent.GetComponent<PlayerStatsComponent>().modifyHealth(10);
    }

    public void getHit(){
        if (getHitTimer <= 0){
        playerStatsComponent.GetComponent<PlayerStatsComponent>().modifyHealth(-1);
        // Debug.Log("Hey I Got Hit!");
        audioSource.PlayOneShot(hitSound);
        canMove = false;
        moveSpeed = -moveSpeed;
        anim.SetFloat("Xdirection", storePreviousMovement[0]);
        anim.SetFloat("Ydirection", storePreviousMovement[1]);
        anim.SetTrigger("isHit");
        getHitTimer = getHitTimerMax;
        }
    }

    void stopGettingHit(){
        canInput = true;
        canMove = true;
        moveSpeed = 0;
    }
    // reset variables for stopping to attack
    void stopAttacking(){
        canMove = true;
        moveSpeed = 0;
        canInput = true;
    }

    private void pickUpItem(){
        if (itemToPickUp.gameObject.tag == "BasicBow"){
            rangedObject = rangedObjects[0];
            rangedAttackOn = true;
            isMagic = false;
            audioSource.PlayOneShot(weaponPickUpSound);
            setPickUpTimer();
        }
        else if (itemToPickUp.gameObject.tag == "FireBow"){
            rangedObject = rangedObjects[1];
            rangedAttackOn = true;
            isMagic = false;
            audioSource.PlayOneShot(weaponPickUpSound);
            setPickUpTimer();
        }
        else if (itemToPickUp.gameObject.tag == "FirePotion"){
            rangedObject = rangedObjects[2];
            rangedAttackOn = true;
            isMagic = true;
            audioSource.PlayOneShot(potionPickUpSound);
            setPickUpTimer();
        }
        else if (itemToPickUp.gameObject.tag == "sword1"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[0]);
        }
        else if (itemToPickUp.gameObject.tag == "sword2"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[1]);
        }
        else if (itemToPickUp.gameObject.tag == "sword3"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[2]);
        }
        else if (itemToPickUp.gameObject.tag == "sword4"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[3]);
        }
        else if (itemToPickUp.gameObject.tag == "sword5"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[4]);
        }
        else if (itemToPickUp.gameObject.tag == "sword6"){
            audioSource.PlayOneShot(weaponPickUpSound);
            // set attack damage to 40
            setAttackDamage(swordPickUpDamages[5]);
        }

        Destroy(itemToPickUp);
        canPickUp = false;
    }

    public void setAttackDamage(int damage){
        attackDamage = damage;
    }

    public int getAttackDamage(){
        return attackDamage;
    }
    public void setPickUpTimer(){
        currentPickupTime = pickupMaxTime;
    }
    // draw wire frames for attack point colliders.
    private void OnDrawGizmos() {
        if (attackPointRight == null){
            return;
        }
        Gizmos.DrawWireSphere(currentAttackPoint.position, attackRange);    
    }
    
    public void LoadPlayer(player_data data){
      currentStamina = data.currentStamina;
      attackDamage = data.attackDamage;
      Vector3 position;
      position.x = data.position[0];
      position.y = data.position[1];
      position.z = data.position[2];
      transform.position = position;
    }
}
