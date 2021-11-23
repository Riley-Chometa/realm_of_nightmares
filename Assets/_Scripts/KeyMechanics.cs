using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMechanics : MonoBehaviour
{
    public Transform transformer;

    public GameObject playerstats;
    
    // Start is called before the first frame update
    void Start()
    {
        transformer = GetComponent<Transform>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        {
            this.playerstats.GetComponent<PlayerStatsComponent>().modifyKeys(1);
            Destroy(this.gameObject);
            
        }
    }
}

