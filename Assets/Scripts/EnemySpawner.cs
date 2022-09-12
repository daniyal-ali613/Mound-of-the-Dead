using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public PlayerController playerController;
    public List<GameObject> enemy = new List<GameObject>();
    int rand;
    int i, counter;

    void Start()
    {
        i = 0;
        counter = 0;
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        do
        {
            yield return new WaitForSeconds(3);

            rand = Random.Range(0, 3);

            if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == false)
            {
                yield return null;
                Debug.Log("Nahi");
                Debug.Log(rand);

            }

            else if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == true)
            {
                enemy[i].SetActive(true);
                enemy[i].transform.position = spawnPoints[rand].position;
                i++;
                Debug.Log("Han");
                Debug.Log(rand);


            }

        } while (counter <= 20);
    }
}
