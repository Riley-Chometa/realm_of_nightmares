using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject target;
    public float searchRange;
    public float attackRadius;
    public Animator anim;
    public float attackCooldown;

    private bool canAttack;
    private bool canMove;
    private enum Direction {
        Up, Down, Left, Right
    }
    private Direction curDirection;


    private Coroutine lastCoroutine;
    private Vector2 targetPosition;
    float speed = 0.011f;
    Vector2[] path;
    int targetIndex;
    Vector2 start;

    protected void Awake(){
        canAttack = true;
        canMove = true;
        start = transform.position;
    }
    void Start(){
        curDirection = Direction.Down;
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = target.transform.position;
        if (Vector2.Distance(transform.position, targetPosition) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        }
        

    }


    protected void Update() {
        
        if (canMove){
            targetPosition = target.transform.position;
            if (Vector2.Distance(transform.position, targetPosition) < searchRange){
                PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
            }
        }

        if (Vector2.Distance(targetPosition, transform.position) < attackRadius && canAttack){
            anim.SetFloat("Speed", 0);
            StopCoroutine(lastCoroutine);
            canAttack = false;
            canMove = false;
            anim.SetTrigger("isAttack");    
            StartCoroutine(Attack());
        }
        

    }
    IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        canMove = true;
        targetPosition = target.transform.position;
        if (Vector2.Distance(targetPosition, transform.position) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
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
            if (lastCoroutine != null){
                StopCoroutine(lastCoroutine);
            }
            lastCoroutine = StartCoroutine(FollowPath());
        }
    }
    /**
    * Follows the path
    */
    IEnumerator FollowPath(){
        if (path.Length > 0){
            Vector2 currentWaypoint = path[0];
            while(true){
                if((Vector2)transform.position == currentWaypoint){
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
