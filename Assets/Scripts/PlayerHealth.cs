using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public PlayerController player;
    public float  maxHealth;
    public bool die;
    public GameObject RestartCanvas;
    public EnemySpawner spawner;
    private void Start()
    {
        die = false;
    }

    public void TakeDamage(int add)
    {
        currentHealth += add;
    }

    private void Update()
    {
        if(currentHealth  >= 100)
        {
            die = true;
            spawner.StopSpawning();
            player.animator.SetBool("death", true);
            StartCoroutine(ActivateCanvas()); 
        }
    }

    IEnumerator ActivateCanvas()
    {
        yield return new WaitForSeconds(7);
        RestartCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}
