using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public AudioClip fireBallSound;
    public GameObject target;
    public GameObject projectile;
    public float searchRange;
    public float attackRadius;
    public bool rangedEnemy;
    private Animator anim;
    public float attackCooldown;
    private float searchCooldown =0.4f;

    private bool canAttack;
    private bool canMove;
    private enum Direction {
        Up, Down, Left, Right
    }
    private Direction curDirection;


    private Coroutine lastCoroutine;
    private Vector2 targetPosition;
    public float speed;
    private float currentSpeed;
    Vector2[] path;
    int targetIndex;
    Vector2 start;
    private bool isDead = false;

    CapsuleCollider2D collider;
    CapsuleCollider2D targetCollider;


    protected void Awake(){
        canAttack = true;
        canMove = true;
        start = transform.position;
    }
    void Start(){
        collider = GetComponent<CapsuleCollider2D>();
        anim = this.GetComponent<Animator>();
        curDirection = Direction.Down;
        target = GameObject.FindGameObjectWithTag("Player");
        targetCollider = target.GetComponent<CapsuleCollider2D>();
        targetPosition = target.transform.position;

    }


    public void setCanMove(bool value){
        canMove = value;
        if (!value){
        StopCoroutine("FollowPath");
        }
    }

    public void SetDead()
    {
        this.isDead = true;
        StopCoroutine("FollowPath");
        if(rangedEnemy){
            StopCoroutine("RangedAttack");
        }else{
            StopCoroutine("Attack");
        }
    }

    protected void FixedUpdate() {
        if (!isDead){
            searchCooldown -= Time.fixedDeltaTime;
            if (Vector2.Distance(target.transform.position, transform.position) < attackRadius && canAttack){
                currentSpeed =0;
                anim.SetFloat("Speed", currentSpeed);
                StopCoroutine("FollowPath");
                canAttack = false;
                canMove = false;
                anim.SetTrigger("isAttack");
                if(rangedEnemy){
                    StartCoroutine("RangedAttack");
                }else{ 
                    StartCoroutine("Attack");
                }
            }
            else if (canMove && searchCooldown <= 0f && Vector2.Distance(transform.position, target.transform.position) < searchRange){
                searchCooldown =0.4f;
                PathRequestManager.RequestPath(transform.position, target.transform.position, this);
            
            }           
        }
    }
    IEnumerator RangedAttack(){
        yield return new WaitForSeconds(0.3f);
        float xdirection = targetPosition.x - transform.position.x;
        float ydirection = targetPosition.y - transform.position.y;
        if (Mathf.Abs(xdirection) > Mathf.Abs(ydirection)){

            anim.SetFloat("Xdirection", xdirection);
            anim.SetFloat("Ydirection", 0);
            
            if(xdirection >0){
                curDirection = Direction.Right;
                GameObject newFireBall = Instantiate(projectile, new Vector3(transform.position.x + 0.5f, transform.position.y, 0), Quaternion.identity);
                if(newFireBall != null){
                    newFireBall.GetComponent<EnemyFireBall>().setTarget(target.transform.position);
                }
            }else{
                curDirection = Direction.Left;
                GameObject newFireBall = Instantiate(projectile, new Vector3(transform.position.x - 0.5f, transform.position.y, 0), Quaternion.identity);
                if(newFireBall != null){
                    newFireBall.GetComponent<EnemyFireBall>().setTarget(target.transform.position);
                }
            }
        }else{
            anim.SetFloat("Xdirection", 0);
            anim.SetFloat("Ydirection", ydirection);
            if(ydirection >0){
                curDirection = Direction.Up;
                GameObject newFireBall = Instantiate(projectile, new Vector3(transform.position.x , transform.position.y + 0.5f, 0), Quaternion.identity);
                if(newFireBall != null){
                    newFireBall.GetComponent<EnemyFireBall>().setTarget(target.transform.position);
                }
            }else{
                curDirection = Direction.Down;
                GameObject newFireBall = Instantiate(projectile, new Vector3(transform.position.x , transform.position.y - 0.5f, 0), Quaternion.identity);
                if(newFireBall != null){
                    newFireBall.GetComponent<EnemyFireBall>().setTarget(target.transform.position);
                }
            }
        }

        StartCoroutine("AttackCooldown");
    }

    IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        canMove = true;
        targetPosition = target.transform.position;
        if (Vector2.Distance(targetPosition, transform.position) < attackRadius){
            float xdirection = targetPosition.x - transform.position.x;
            float ydirection = targetPosition.y - transform.position.y;
            if (Mathf.Abs(xdirection) > Mathf.Abs(ydirection)){

                anim.SetFloat("Xdirection", xdirection);
                anim.SetFloat("Ydirection", 0);
                if(xdirection >0){
                    curDirection = Direction.Right;
                }else{
                    curDirection = Direction.Left;
                }
            }else{
                anim.SetFloat("Xdirection", 0);
                anim.SetFloat("Ydirection", ydirection);
                if(ydirection >0){
                    curDirection = Direction.Up;
                }else{
                    curDirection = Direction.Down;
                }
            }
        }
        
    }

    IEnumerator Attack(){
        yield return new WaitForSeconds(0.3f);
        Vector2 attackPosition = transform.position;
        if(curDirection == Direction.Up){
            attackPosition.y += attackRadius - 0.2f;
        }else if(curDirection == Direction.Down){
            attackPosition.y += -attackRadius + 0.2f;
        }else if(curDirection == Direction.Right){
            attackPosition.x += attackRadius - 0.2f;
        }else{
            attackPosition.x += -attackRadius + 0.2f;
        }
        if (Vector2.Distance(target.transform.position, attackPosition) < attackRadius){
            target.GetComponent<PlayerMovement>().getHit();
        }
        StartCoroutine("AttackCooldown");
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful){
        if (pathSuccessful){
            path = newPath;
            // if (lastCoroutine != null){
            StopCoroutine("FollowPath");
            // }
            //lastCoroutine = 
            StartCoroutine("FollowPath");
        }
    }
    /**
    * Follows the path
    */
    IEnumerator FollowPath(){
        if (path.Length > 0){
            targetIndex =0;
            Vector2 currentWaypoint = path[0];
            currentSpeed = speed;
            Vector2 previousPosition = transform.position;
            while(true){
                Debug.DrawLine(transform.position, currentWaypoint, Color.white, 2f, false);
                if(Vector2.Distance(transform.position, currentWaypoint) <= 0.1f){
                    targetIndex++;
                    if(targetIndex >= path.Length){
                        targetIndex = 0;
                        yield break;
                    }
                currentWaypoint = path[targetIndex];
                }
                anim.SetFloat("Speed", currentSpeed);
                float xdirection = currentWaypoint.x - transform.position.x;
                float ydirection = currentWaypoint.y - transform.position.y;
                if (Mathf.Abs(xdirection) > Mathf.Abs(ydirection)){
                    anim.SetFloat("Xdirection", xdirection);
                    anim.SetFloat("Ydirection", 0);
                    if(xdirection >0){
                        curDirection = Direction.Right;
                    }else{
                        curDirection = Direction.Left;
                    }
                }
                else {
                    anim.SetFloat("Xdirection", 0);
                    anim.SetFloat("Ydirection", ydirection);
                    if(ydirection >0){
                        curDirection = Direction.Up;
                    }else{
                        curDirection = Direction.Down;
                    }
                }
            while(Time.timeScale == 0){
                yield return null;
            }
            previousPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, currentSpeed);
            yield return null;
            }
        }
        
    }
    private void OnDrawGizmos() {
        if(!rangedEnemy){
            Gizmos.DrawWireSphere(transform.position, searchRange);
            switch(curDirection){
                case Direction.Up:{
                    Vector2 attackPosition = new Vector2(transform.position.x, transform.position.y + attackRadius- 0.2f);
                    Gizmos.DrawWireSphere(attackPosition, attackRadius); 
                    break;
                }
                case Direction.Down:{
                    Vector2 attackPosition = new Vector2(transform.position.x, transform.position.y - attackRadius+ 0.2f);
                    Gizmos.DrawWireSphere(attackPosition, attackRadius); 
                    break;
                }
                case Direction.Right:{
                    Vector2 attackPosition = new Vector2(transform.position.x + attackRadius- 0.2f, transform.position.y);
                    Gizmos.DrawWireSphere(attackPosition, attackRadius); 
                    break;
                }
                case Direction.Left:{
                    Vector2 attackPosition = new Vector2(transform.position.x - attackRadius+ 0.2f, transform.position.y);
                    Gizmos.DrawWireSphere(attackPosition, attackRadius); 
                    break;
                }
            }
        }   
    }
    
}
