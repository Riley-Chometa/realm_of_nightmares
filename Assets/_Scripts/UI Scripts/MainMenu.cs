using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("ControlScreen");
        GameObject GameMode = GameObject.Find("GameMode");
        GameMode.GetComponent<GameMode>().SetAustralianMode(false);
        DontDestroyOnLoad(GameMode);
    }
    public void PlayAustralianMode()
    {
        SceneManager.LoadScene("ControlScreen");
        GameObject GameMode = GameObject.Find("GameMode");
        GameMode.GetComponent<GameMode>().SetAustralianMode(true);
        DontDestroyOnLoad(GameMode);
    }
    public void QuitGame(){
      Application.Quit();
    }
}
