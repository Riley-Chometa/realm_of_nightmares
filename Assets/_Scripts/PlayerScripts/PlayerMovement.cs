using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    // Constants for movement speed and jump
    [SerializeField]
    private float moveSpeedMax = 7.0f;
    private float moveSpeed = 0.0f;
    [SerializeField]
    private float sprintMaxSpeed = 20.0f;
    [SerializeField]
    private int maxStamina = 80;
    [SerializeField]
    private int currentStamina;
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
    // [SerializeField]
    // private float jumpSpeed = 5.0f;

    private bool canMove;
    private bool isAlive;
    // Setup attributes and components needed for movement of player.
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 storePreviousMovement;

    public Animator anim;
    public StaminaBar staminaBar;

    // ground check boolean
    // private bool isGrounded;

    // [SerializeField]
    // public Transform groundCheck;

    //Attack Variables
    public int attackDamage = 20;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public Transform attackPointUp;
    public Transform attackPointDown;

    public Transform currentAttackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;

    public AudioClip attackSwingSound;
    public AudioClip hitSound;
    private AudioSource audioSource;

    // Initialize variables for the player.
    void Start(){
        storePreviousMovement = new Vector2(0,0);
        currentAttackPoint = attackPointDown;
        currentStamina = maxStamina;
        staminaBar.setStamina(maxStamina);
        staminaBar.SetMaxValue(maxStamina);
        canMove = true;
        isAlive = true;
        audioSource = GetComponent<AudioSource>();
    } 

    // Update is called once per frame, Contains primary input from player.
    void Update()
    {   if (isAlive){
            if (canMove){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            }
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Speed", movement.sqrMagnitude);

            float xdirection = movement.x;
            float ydirection = movement.y;
            // simple movement information
            if (canMove){
                if (movement.magnitude > .02){
                    if (Mathf.Abs(xdirection) > Mathf.Abs(ydirection)){
                        anim.SetFloat("Xdirection", xdirection);
                        anim.SetFloat("Ydirection", 0);
                    }
                    else {
                        anim.SetFloat("Xdirection", 0);
                        anim.SetFloat("Ydirection", ydirection);
                    }
                }
            }
            // simple spaceBar attack.
            if (Input.GetKeyDown(KeyCode.Space)){
                canMove = false;
                Attack();
            }

            if (Input.GetKeyDown("e")){
                getHit();
            }
            if (Input.GetKeyDown("t")){
                canMove = false;
                playerDie();
            }
        }
    }

    /**
    Main FixedUpdate Method for movement, includes sprinting mechanic and stamina bar.
    */
    void FixedUpdate(){
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
        // Debug.Log("Collided with: " + col.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag != "Coins" || other.gameObject.tag != "PickUps"){
            moveSpeed = 0;
        }    
    }

    // Make the player attack.
    void Attack(){
        anim.SetFloat("Xdirection", storePreviousMovement[0]);
        anim.SetFloat("Ydirection", storePreviousMovement[1]);
        anim.SetTrigger("attack");
        // set which attack point to use during attack animation.
        // this is right quadrant.
        if (storePreviousMovement[0] > 0 && storePreviousMovement[1] < .5 && storePreviousMovement[1] >-.5){
            currentAttackPoint = attackPointRight;
        }
        // this is left quadrant
        else if (storePreviousMovement[0] <= 0 && storePreviousMovement[1] < .5 && storePreviousMovement[1] > -.5){
            currentAttackPoint = attackPointLeft;
        }
        // bottom Quadrant
        else if (storePreviousMovement[1] < 0 && storePreviousMovement[0] < .5 && storePreviousMovement[0] > -.5){
            currentAttackPoint = attackPointDown;
        }
        // top Quadrant
        else {
            currentAttackPoint = attackPointUp;
        }
    }

    void attackSwingAudio(){
        audioSource.PlayOneShot(attackSwingSound);
    }

    // call the actual part of the animation where damage occurs.
    void attackAnimation(){
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(currentAttackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in enemiesHit){
            enemy.GetComponent<BaseEnemy>().TakeDamage(attackDamage);
        }
    }

    // potential mechanic to think about adding.
    void makeCharJump(){

    }

    private void playerDie(){
        isAlive = false;
        canMove = false;
        moveSpeed = 0;
        anim.SetFloat("Xdirection", storePreviousMovement[0]);
        anim.SetFloat("Ydirection", storePreviousMovement[1]);
        anim.SetTrigger("isDead");
    }

    public void getHit(){
        // Debug.Log("Hey I Got Hit!");
        audioSource.PlayOneShot(hitSound);
        canMove = false;
        moveSpeed = -moveSpeed;
        anim.SetFloat("Xdirection", storePreviousMovement[0]);
        anim.SetFloat("Ydirection", storePreviousMovement[1]);
        anim.SetTrigger("isHit");
    }

    void stopGettingHit(){
        canMove = true;
        moveSpeed = 0;
    }
    // reset variables for stopping to attack
    void stopAttacking(){
        canMove = true;
        // purposely trying to break attacking
        moveSpeed = 0;
    }

    // draw wire frames for attack point colliders.
    private void OnDrawGizmos() {
        if (attackPointRight == null){
            return;
        }
        Gizmos.DrawWireSphere(currentAttackPoint.position, attackRange);    
    }
}
