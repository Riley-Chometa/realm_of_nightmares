using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    
    private Vector3 targetPosition;
    float speed = 0.1f;
    Vector3[] path;
    int targetIndex;
    Vector3 start;

    void Awake(){
        start = transform.position;
    }
    void Start(){
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        targetPosition = target.position;

    }


    private void Update() {
        if (Vector3.Distance(target.position, targetPosition) != 0){
            targetPosition = target.position;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
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
            Vector3 currentWaypoint = path[0];

            while(true){
                if(transform.position == currentWaypoint){
                    targetIndex++;
                    if(targetIndex >= path.Length){
                        targetIndex = 0;
                        yield break;
                    }
                currentWaypoint = path[targetIndex];
                }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
            }
        }
        
    }
    
}
