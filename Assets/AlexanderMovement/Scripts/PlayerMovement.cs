using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed; // For holding the float value associated with how fast the player goes. 
    public Rigidbody2D rb;
    public Animator animator; //for animating the player
    private Vector2 playerDirection; // for the holding which direction the player is facing. 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get inputs from the user
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(moveX, moveY).normalized; // Add normalized to keep the movement speed of the player consistent

        //Animate the character
        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveY);
        animator.SetFloat("Speed", playerDirection.sqrMagnitude); //Use sqrMagnitude to regulate the speed of the animations

    }

    // Helps the player sprite move at specific speeds
    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerDirection.x * movementSpeed, playerDirection.y * movementSpeed); // allows our player to move at a specific speed. 
    }


}
