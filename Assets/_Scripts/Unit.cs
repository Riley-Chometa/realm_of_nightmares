using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject target;
    public float searchRange;
    public float attackRadius;
    private Animator anim;
    public float attackCooldown;

    private bool canAttack;
    private bool canMove;
    private enum Direction {
        Up, Down, Left, Right
    }
    private Direction curDirection;


    private Coroutine lastCoroutine;
    private Vector2 targetPosition;
    public float speed = 0.02f;
    Vector2[] path;
    int targetIndex;
    Vector2 start;
    private bool isDead = false;


    protected void Awake(){
        canAttack = true;
        canMove = true;
        start = transform.position;
    }
    void Start(){
        anim = this.GetComponent<Animator>();
        curDirection = Direction.Down;
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = target.transform.position;
        if (Vector2.Distance(transform.position, targetPosition) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, this);
        }
        

    }

    public void setCanMove(bool value){
        canMove = value;
        if (!value){
        StopCoroutine("FollowPath");
        }
        else {
            if (Vector2.Distance(transform.position, targetPosition) < searchRange){
                    PathRequestManager.RequestPath(transform.position, targetPosition, this);
            }
        }
    }

    public void SetDead()
    {
        this.isDead = true;
        StopCoroutine("FollowPath");
        StopCoroutine(Attack());
    }

    protected void FixedUpdate() {
        if (!isDead){
            if (canMove && Vector2.Distance(targetPosition, target.transform.position) != 0){
                targetPosition = target.transform.position;
                if (Vector2.Distance(transform.position, targetPosition) < searchRange){
                    PathRequestManager.RequestPath(transform.position, targetPosition, this);
                }
            }

            if (Vector2.Distance(targetPosition, transform.position) < attackRadius && canAttack){
                anim.SetFloat("Speed", 0);
                StopCoroutine("FollowPath");
                canAttack = false;
                canMove = false;
                anim.SetTrigger("isAttack");    
                StartCoroutine(Attack());
            }
        }else{
            StopCoroutine("FollowPath");
            StopCoroutine(Attack());
        }
            

    }
    IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        canMove = true;
        targetPosition = target.transform.position;
        if (Vector2.Distance(targetPosition, transform.position) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, this);
        }
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
            Vector2 currentWaypoint = path[0];
            while(true){
                Debug.DrawLine(transform.position, currentWaypoint, Color.white, 2f, false);
                if(Vector2.Distance(transform.position, currentWaypoint) < speed){
                    targetIndex++;
                    if(targetIndex >= path.Length){
                        targetIndex = 0;
                        yield break;
                    }
                currentWaypoint = path[targetIndex];
                }
                anim.SetFloat("Speed", speed);
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
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
            }
        }
        
    }
    private void OnDrawGizmos() {

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
