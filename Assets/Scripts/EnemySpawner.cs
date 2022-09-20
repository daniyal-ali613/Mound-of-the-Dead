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
    int rand;
    int i, counter;

    void Start()
    {
        i = 0;
        counter = 0;
        StartCoroutine(StartSpawning());
    }

    private void Update()
    {
        if(counter >= 20)
        {
            Restart.SetActive(true);
            counter = 0; 
        }

    }

    
    IEnumerator StartSpawning()
    {
        do
        {
            yield return new WaitForSeconds(5);

            rand = Random.Range(0, 3);

            if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == false)
            {
                yield return null;
                Debug.Log(rand);

            }

            else if(spawnPoints[rand].GetComponent<SpawnPointStatus>().GetSpawn() == true)
            {
                
                enemy[i].SetActive(true);
                GameObject.FindObjectOfType<PlayerController>().AddTarget(enemy[i]);
                AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position);
                enemy[i].transform.position = spawnPoints[rand].position;
                i++;
            }

        } while (counter <= 20);
    }
}
