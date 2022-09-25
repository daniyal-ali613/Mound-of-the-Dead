using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    TextMeshProUGUI txt;
    EnemySpawner gameScore;
    int finalScore;
    static int bestScore = 0;
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        gameScore = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        finalScore = gameScore.GetScore();

        if (finalScore > bestScore)
        {
            bestScore = finalScore;
        }

        txt.text = bestScore.ToString();
    }

}
