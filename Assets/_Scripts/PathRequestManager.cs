using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake(){
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Unit enemy){
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, enemy);
        instance.PathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext(){
        if(!isProcessingPath && PathRequestQueue.Count > 0){
            currentPathRequest = PathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPAth(Vector2[] path, bool success){
        if (currentPathRequest.enemy != null){
            currentPathRequest.enemy.OnPathFound(path,success);
            isProcessingPath = false;
            TryProcessNext();
        }
    }
    struct PathRequest {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Unit enemy;

        public PathRequest(Vector2 start, Vector2 end, Unit call){
            pathStart = start;
            pathEnd = end;
            enemy = call;
        }
    }
}
