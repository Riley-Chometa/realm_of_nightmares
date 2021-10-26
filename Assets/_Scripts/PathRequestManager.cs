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
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback){
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
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
        currentPathRequest.callback(path,success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> call){
            pathStart = start;
            pathEnd = end;
            callback = call;
        }
    }
}
