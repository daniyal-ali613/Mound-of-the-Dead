using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        SetUpSingelton();
    }

    private void SetUpSingelton()
    {
        int numberOfMusicPlayer = FindObjectsOfType<MusicPlayer>().Length;
        if (numberOfMusicPlayer > 1)
        {
            Destroy(gameObject);
        }
    }

}
