using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Escape)){
        if(GameIsPaused){
          Resume();
        }
        else{
          Pause();
        }
      }
        
    }
    public void Resume(){
      pauseMenu.SetActive(false);
      Time.timeScale = 1.0f;
      GameIsPaused = false;
    }
    public void Pause(){
      pauseMenu.SetActive(true);
      Time.timeScale = 0.0f;
      GameIsPaused = true;
    }
}
