using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject target;
    public float searchRange;
    public float attackRadius;

    private bool canAttack;

    private Vector2 targetPosition;
    float speed = 0.01f;
    Vector2[] path;
    int targetIndex;
    Vector2 start;

    void Awake(){
        canAttack = true;
        start = transform.position;
    }
    void Start(){
        target = GameObject.FindGameObjectWithTag("Player");
        targetPosition = target.transform.position;
        if (Vector2.Distance(transform.position, targetPosition) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        }
        

    }


    private void Update() {
        
        if (Vector2.Distance(target.transform.position, targetPosition) != 0){
            targetPosition = target.transform.position;
            if (Vector2.Distance(transform.position, targetPosition) < searchRange){
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
            
            }
        }

        if (Vector2.Distance(targetPosition, transform.position) < attackRadius && canAttack && gameObject.layer == 10){
            StopCoroutine("FollowPath");
            canAttack = false;
            Attack();
            StartCoroutine("AttackCooldown");
            
        }

    }
    IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    private void Attack(){
        target.GetComponent<PlayerMovement>().getHit();
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful){
        if (pathSuccessful){
            path = newPath;
            StopCoroutine("FollowPath");
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
                if((Vector2)transform.position == currentWaypoint){
                    targetIndex++;
                    if(targetIndex >= path.Length){
                        targetIndex = 0;
                        yield break;
                    }
                currentWaypoint = path[targetIndex];
                }
            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
            }
        }
        
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, attackRadius);    
    }
    
}
