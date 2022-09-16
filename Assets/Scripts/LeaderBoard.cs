using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{score= 523456, name="AAA"},
            new HighscoreEntry{score= 3456, name="DAV"},
            new HighscoreEntry{score= 23456, name="LASA"},
            new HighscoreEntry{score= 5234, name="JOE"},
            new HighscoreEntry{score= 73456, name="MAX"},
        };

        //AddHighscoreEntry(10000, "cmk");
        //AddHighscoreEntry(100000, "abk");
        //AddHighscoreEntry(110000, "hmk");

        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        //sort entry list by score
        for (int i = 0; i < highScores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highscoreEntryList.Count; j++)
            {
                if (highScores.highscoreEntryList[j].score > highScores.highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highScores.highscoreEntryList[i];
                    highScores.highscoreEntryList[i] = highScores.highscoreEntryList[j];
                    highScores.highscoreEntryList[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHeighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

     
    }
    private void CreateHeighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        string name = highscoreEntry.name;

        entryTransform.Find("nameText").GetComponent<Text>().text = name;
        if (rank == 1)
        {
            //Highlight First
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        transformList.Add(entryTransform);
    }
    private void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        Debug.Log(jsonString);

        //add new entry to highscores
        highScores.highscoreEntryList.Add(highscoreEntry);

        //save updated highscores
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }
    private class HighScores
    {
        public List<HighscoreEntry> highscoreEntryList;

    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
