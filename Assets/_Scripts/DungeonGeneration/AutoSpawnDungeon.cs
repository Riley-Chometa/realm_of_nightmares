using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

public class AutoSpawnDungeon : MonoBehaviour
{
    [SerializeField]
    private RoomFirstDungeonGenerator generator;
    [SerializeField]
    private Animator animator;
    private static bool inStore = false;
    private GameObject Store;
    private GameObject player;
    private TransitionTextScript transitionText;
    [SerializeField]
    private GameObject statCanvas;
    private int EnemyBaseHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        transitionText = GameObject.Find("SpawnDungeon").GetComponentInChildren<TransitionTextScript>();
        //transitionText.SetLevelText();
        Store = GameObject.Find("StorePlayerSpawn");
        player = GameObject.Find("player");
        generator = GameObject.Find("RoomsFirstDungeonGenerator").GetComponent<RoomFirstDungeonGenerator>();
        generator.MakeDungeon();
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            
            if (!inStore)
            {
                inStore = true;
                GameObject.Find("StatTracker").GetComponent<StatTracker>().endOfLevel();
                //SceneManager.LoadScene("TempStoreScene");
                player.transform.position = new Vector3(Store.transform.position.x, Store.transform.position.y, 0);
                animator.SetTrigger("StartTransition");
                toggleStore();
            }
            else if (inStore)
            {
                generator.level++;
                inStore = false;
                animator.SetTrigger("StartTransition");
                generator.MakeDungeon();
                int level = generator.getLevel();
                BaseEnemy[] enemies = FindObjectsOfType(typeof(BaseEnemy)) as BaseEnemy[];
                foreach (BaseEnemy enemy in enemies){
                    enemy.increaseHealth(EnemyBaseHealth + level*15);
                }
                //transitionText.toggleInStore();
            }
            
            // generator.GenerateDungeon();
        }

    }


    private IEnumerator toggleStore()
    {
        yield return new WaitForSeconds(10);
        transitionText.toggleInStore();
    }
}


