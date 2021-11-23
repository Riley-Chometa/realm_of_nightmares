using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRock : MonoBehaviour
{
    //animator to access the broken rock animation
    public Animator anim;
    public AudioSource source;
    public AudioClip rockSmash;

    public void destroyRock()
    {
        source.PlayOneShot(rockSmash);
        anim.Play("Breakablerockdestroyed"); 
        Destroy(gameObject.GetComponent<BoxCollider2D>()); //Allow the player to walk ontop of this object as it is no longer an obstacle.
    }
}
