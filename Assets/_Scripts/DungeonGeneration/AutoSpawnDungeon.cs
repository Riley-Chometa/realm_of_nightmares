using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class AutoSpawnDungeon : MonoBehaviour
{
    [SerializeField]
    private RoomFirstDungeonGenerator generator;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject statCanvas;
    // Start is called before the first frame update
    void Start()
    {
        generator.GenerateDungeon();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(statCanvas, transform.position, transform.rotation);
            animator.SetTrigger("StartTransition");
            GameObject.Find("StatTracker").GetComponent<StatTracker>().endOfLevel();
            generator.GenerateDungeon();
        }
    }
}


