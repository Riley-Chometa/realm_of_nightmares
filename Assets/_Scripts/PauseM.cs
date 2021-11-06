using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseM : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.P)){
          if(isPaused){
            Resume();
          }
          else{
            Pause();
          }
        }
    }
    public void Resume(){
      PauseMenu.SetActive(false);
      Time.timeScale = 1f;
      isPaused = false;
    }
    void Pause(){
      PauseMenu.SetActive(true);
      Time.timeScale = 0f;
      isPaused = true;
    }

    public void LoadMenu(){

    }
    public void QuitGame(){
      Application.Quit();
    }
}
