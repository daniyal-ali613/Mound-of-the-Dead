using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int damage;
    public Animator animator;
    public Rigidbody2D rb;
    void Start()
    {
        this.damage = 4;
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

    IEnumerator  murderedEnemy()
    {
        yield return new  WaitForSeconds(2f);

        //this.gameObject.SetActive(false);
    }
}
