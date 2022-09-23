using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{


    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadCutScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(3);
    }



    public void QuitGame()
    {
        Application.Quit();
    }

}
