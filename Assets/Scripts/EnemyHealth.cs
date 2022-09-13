using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int damage;
    public Animator animator;
    void Start()
    {
        damage = 6;
    }

    public void TakeDamage(int subtract )
    {
        damage -= subtract;
    }

    private void Update()
    {
        if(this.damage <= 0)
        {
            animator.SetBool("death", true);
        }
    }
}
