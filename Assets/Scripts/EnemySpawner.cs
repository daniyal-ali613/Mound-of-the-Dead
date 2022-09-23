using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public PlayerController playerController;
    public List<GameObject> enemy = new List<GameObject>();
    public AudioClip spawnSound;
    public GameObject Restart;
    public float timeToSpawn;
    int rand;
    int i, counter;

    void Start()
    {
        i = 0;
        counter = enemy.Count;
        StartCoroutine(StartSpawning());
    }

    private void Update()
    {
        if(i > counter)
        {
            Restart.SetActive(true);
            counter = 0; 
        }
    }

    
    IEnumerator StartSpawning()
    {
        do
        {
            yield return new WaitForSeconds(timeToSpawn);

            rand = Random.Range(0, 3);

            if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == false)
            {
                yield return null;
            }

            else if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == true)
            {
                
                enemy[i].SetActive(true);
                GameObject.FindObjectOfType<PlayerController>().AddTarget(enemy[i]);
                AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position);
                enemy[i].transform.position = spawnPoints[rand].position;
                i++;
            }

        } while (i < counter);
    }
}
