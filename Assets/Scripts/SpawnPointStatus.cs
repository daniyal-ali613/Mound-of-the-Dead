using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointStatus : MonoBehaviour
{
    bool spawning;
    void Start()
    {
        spawning = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Walls"))
        {
            spawning = false;
        }

        else
        {
            spawning = true;
        }
        
    }

    public bool GetSpawn()
    {
        return spawning;
    }

}
