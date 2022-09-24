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
            player.animator.SetBool("death", true);
            die = true;
            RestartCanvas.SetActive(true);
        }
    }
}
