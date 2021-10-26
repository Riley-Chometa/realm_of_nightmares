using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject target;
    
    private Vector2 targetPosition;
    float speed = 2f;
    Vector2[] path;
    int targetIndex;
    Vector2 start;

    void Awake(){
        start = transform.position;
    }
    void Start(){
        targetPosition = target.transform.position;
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        

    }


    private void Update() {
        if (Vector2.Distance(target.transform.position, targetPosition) != 0){
            targetPosition = target.transform.position;
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        }

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
    
}
