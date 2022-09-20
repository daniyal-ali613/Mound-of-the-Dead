using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int damage;
    public Animator animator;
    public Rigidbody2D rb;
    bool killed;
    void Start()
    {
        this.damage = 4;
        killed = false;
    }

    public void TakeDamage(int subtract )
    {
        this.damage -= subtract;
    }

    private void Update()
    {
        if(this.damage <= 0)
        {
            animator.SetBool("death", true);
            this.rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(murderedEnemy());
        }
    }

    public bool killChecker()
    {
        return killed;
    }

    IEnumerator  murderedEnemy()
    {
        yield return new  WaitForSeconds(2f);
        killed = true;
    }

    
}
