using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRock : MonoBehaviour
{
    //animator to access the broken rock animation
    public Animator anim;
    public AudioSource source;
    public AudioClip rockSmash;

    public int rockHP = 5; // variable for keeping track of how many sword hits it takes for the player to destroy the rock object. 

    // This helps to change the state of the rock. 
    public void Update()
    {
        if (rockHP == 4)
        {
            anim.Play("Rockstatesmashed2");
        }
        if (rockHP == 3)
        {
            anim.Play("Rockstatesmashed3");
        }
        if (rockHP == 2)
        {
            anim.Play("Rockstatesmashed4");
        }
        if (rockHP == 1)
        {
            anim.Play("Rockstatesmashed5");
        }
        if (rockHP <= 0){
            destroyRock();
        }
    }


    //Decrease the variable rockHP by 1
    public void rockHit(int damagae)
    {
        rockHP -= damagae;
        //Debug.Log("Current RockHP = "+ rockHP);
    }

    public void destroyRock()
    {
        //source.PlayOneShot(rockSmash);
        anim.Play("Breakablerockdestroyed"); 
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(gameObject.GetComponent<BoxCollider2D>()); //Allow the player to walk ontop of this object as it is no longer an obstacle.

    }
}
