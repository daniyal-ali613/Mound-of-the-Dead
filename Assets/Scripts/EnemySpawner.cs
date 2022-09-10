using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public PlayerController playerController;
    public List<GameObject> enemy = new List<GameObject>();
    SpawnPointStatus spawnPointStatus;
    int rand;
    int i, counter;

    void Start()
    {
        i = 0;
        counter = 0;
        spawnPointStatus = FindObjectOfType<SpawnPointStatus>();
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        do
        {
            yield return new WaitForSeconds(10);

            rand = Random.Range(0, 3);
            Collider2D spawnStatus;
            spawnStatus = spawnPoints[rand].GetComponent<Collider2D>();

            if(spawnPointStatus.GetSpawn() == false)
            {
               yield return null;
            }

            else
            {
                enemy[i].SetActive(true);
                enemy[i].transform.position = spawnPoints[rand].position;
                i++;
            }
           
        } while (counter <= 20);
    }
}
