using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Level Score")]
    public int count = 0;
    public TextMeshProUGUI enemiesCount;
    public TextMeshProUGUI playerScore;
    float playercount ;
    float currentCount;

    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        

    }

    // Update is called once per frame
    void Update()
    {
       


    }

    public void PlayerScore()
    {
        enemiesCount.text = count + "";
        playercount = count * 4;
        currentCount = PlayerPrefs.GetFloat("TotalScore") + playercount;
        playerScore.text = currentCount + "";
        if(PlayerPrefs.GetFloat("TotalScore") <= playercount)
       PlayerPrefs.SetFloat("TotalScore", currentCount);

    }
}
