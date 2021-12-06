using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScreen : MonoBehaviour
{
    
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainDungeon"); //Change to the maindungeon scene once the player has read the instructions. 
        }
    }
}
