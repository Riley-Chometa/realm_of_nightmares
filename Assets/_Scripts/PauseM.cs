using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseM : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;
    PlayerMovement player;
    private void Start() {
      isPaused = false;
      player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
      Resume();
    }

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
      player.canInput = true;
      Time.timeScale = 1f;
      isPaused = false;
    }
    void Pause(){
      PauseMenu.SetActive(true);
      player.canInput = false;
      Time.timeScale = 0f;
      isPaused = true;
    }
    public void QuitGame(){
      SceneManager.LoadScene("MainMenu");
    }
}
