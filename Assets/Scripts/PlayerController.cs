using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    public EnemyAI enemyAI;
    public EnemyHealth enemyHealth;
    public GameObject spawnpoint1;
    public GameObject spawnpoint2;
    public GameObject spawnpoint3;
    public GameObject spawnpoint4;
    public Vector2 movement;

  
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);



        if (Input.GetMouseButtonDown(0) && movement.x > 0)
        {
            animator.SetBool("attackRight", true);
        }

        else if (Input.GetMouseButtonDown(0) && movement.x < 0)
        {
            animator.SetBool("attackLeft", true);
        }

        else if (Input.GetMouseButtonDown(0) && movement.y > 0)
        {
           
            animator.SetFloat("attackUp", 1);
        }

        else if (Input.GetMouseButtonDown(0) && movement.y < 0)
        {
          
            animator.SetFloat("attackDown", 1);
        }

        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetFloat("attackDown", 1);
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void CancelAttackAnimation()
    {
        animator.SetFloat("attackRight", 0);
        animator.SetFloat("attackLeft", 0);
        animator.SetFloat("attackUp", 0);
        animator.SetFloat("attackDown", 0);
    }

    public void PlayerAttackDetector()
    {
        if (enemyAI.dist < enemyAI.minHorizotalDistance && this.movement.x > 0 && enemyAI.direction.x > 0)
        {
            Debug.Log("attack");
            enemyHealth.TakeDamage(1);


            
        }

        else if (enemyAI.dist < enemyAI.minVerticleDistance && this.movement.y > 0 && enemyAI.direction.y > 0)
        {
            Debug.Log("Attack");
            enemyHealth.TakeDamage(1);
         
            

        }
    }

}
