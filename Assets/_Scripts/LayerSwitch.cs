using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Purpose of this class is to change the sorting layer of the player. example. Walk in front and behind the same tree.
*/
public class LayerSwitch : MonoBehaviour
{   
    // Variables for switch.
    public Transform tm;
    // hard coded ahead and below layers. These are sprite sorting orders. Player is on layer 8.
    private int ahead = 9;
    private int below = 7;
    private SpriteRenderer sprite;
    
    // get components.
    void Start()
    {
        tm = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // on collision enter check the other objects position and change the sorting layer based on current position of collision.
    void OnCollisionEnter2D(Collision2D col){
        GameObject otherObject = col.gameObject;
        if (otherObject.transform.position.y + .25 < tm.position.y){
            sprite.sortingOrder = below;
        }
        else {
            sprite.sortingOrder = ahead;
        }
    }
}
