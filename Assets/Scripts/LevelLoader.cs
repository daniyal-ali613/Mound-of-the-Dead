using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
   
    public void RestartScene()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void LoadMainMenuFromCredits()
    {
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



    public void ScoreScene()
    {
        SceneManager.LoadScene(4);
    }

}
