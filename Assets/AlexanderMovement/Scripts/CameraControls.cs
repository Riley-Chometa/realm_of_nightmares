using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform playerPosi; // holds the position that the player is currently at
    public float smoother; // for smoothing the movement of our camera. 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate() 
    {
        if(transform.position != playerPosi.position) //If the camera's position isn't at the same position as the player entity the camera will move to that position
        {
            Vector3 targetPosi = new Vector3(playerPosi.position.x, playerPosi.position.y, transform.position.z); //Vector3 for where we want our camera to end up
            transform.position = Vector3.Lerp(transform.position, targetPosi, smoother); // lerp is used to help the camera smoothly catchup to the player. 
        }
    }
}
