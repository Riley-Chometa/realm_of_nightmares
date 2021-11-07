using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDoorMechanics : MonoBehaviour
{

    //Variables used to play sound effects. Mainly when the player interacts with this gameobject. 
    public AudioSource source;
    public AudioClip sizzleSound;
    public AudioClip steamSound;
    public Animator animFire; //animator object for the various fire animations. Mainly to change from its default to the "Idle" state once the player has cleared that floor.  
    public Animation fireLoop;

    private void Update()
    {
        // A simple if statement for testing the fireExtinguisher function. Gets rid of all the fire doors. 
        if (Input.GetKeyDown("h")) 
        {
            fireExtinguisher();
        }
    }

    void OnCollisionEnter2D(Collision2D coli)
    {
        //if (coli.gameObject.name == "player (1)") is used for testing purposes. 
        if (coli.gameObject.name == "player (1)") 
        {
            source.PlayOneShot(sizzleSound);
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().getHit(); //Cause the player to take damage once they make contact with the pillar of fire. 
            
        }
    }

    public void fireExtinguisher()
    {
        source.PlayOneShot(steamSound);
        animFire.Play("Fire_Idle"); //Change the animation to the idle state. 
        Destroy(gameObject.GetComponent<BoxCollider2D>()); //Allow the player to walk ontop of this object as it is no longer an obstacle. 
        Destroy(GetComponent<Transform>().GetChild(0).gameObject); //Destroys the Light 2D child object 

    }
}
