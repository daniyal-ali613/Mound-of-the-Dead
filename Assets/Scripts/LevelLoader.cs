using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    MusicPlayer music;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        music = FindObjectOfType<MusicPlayer>();
    }
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
        DontDestroyOnLoad(music);
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
        DontDestroyOnLoad(music);
    }



    public void QuitGame()
    {
        Application.Quit();
    }

}
