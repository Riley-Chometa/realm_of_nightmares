using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMechanics : MonoBehaviour
{

    public Transform transformer; 

    // Start is called before the first frame update
    void Start()
    {
        transformer = GetComponent<Transform>();    
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D coli)
    {
        if(coli.gameObject.name == "player")
        {
            //Check to see if the player has more than one key 
            if (GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().getNumKeys() >= 1)
            {
                Destroy(gameObject);
                GameObject.FindWithTag("CoinUpdater").GetComponent<PlayerStats>().modifyKeys(-1);
            }
        }
    }
}
