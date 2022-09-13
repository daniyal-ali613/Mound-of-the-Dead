using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    public EnemyAI enemyAI;
    public GameObject spawnpoint1;
    public GameObject spawnpoint2;
    public GameObject spawnpoint3;
    public GameObject spawnpoint4;
    Coroutine blinkRoutine;

    public Vector2 movement;

    private void Awake()
    {
        blinkRoutine = StartCoroutine(BlinkAnimation());
    }
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
            animator.SetBool("attackUp", true);
        }

        else if (Input.GetMouseButtonDown(0) && movement.y < 0)
        {
            animator.SetBool("attackDown", true);
        }

        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("attackDown", true);
        }


    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void CancelAttackAnimation()
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackUp", false);
        animator.SetBool("attackDown", false);
    }

    public void AttackDetector()
    {
        if (enemyAI.dist < enemyAI.minDistance && this.movement.x > 0 && enemyAI.direction.x > 0)
        {
            Debug.Log("attack");
        }

        else if (enemyAI.dist < enemyAI.minDistance && this.movement.y > 0 && enemyAI.direction.y > 0)
        {
            Debug.Log("Attack");

        }
    }

    public void Blink()
    {
        if (enemyAI.dist < enemyAI.minDistance && this.movement.x > 0 && enemyAI.direction.x > 0)
        {
            if (blinkRoutine == null)
            {

                StartCoroutine(BlinkAnimation());

            }

            else if (enemyAI.dist < enemyAI.minDistance && this.movement.y > 0 && enemyAI.direction.y > 0)
            {
                if (blinkRoutine == null)
                {
                    StartCoroutine(BlinkAnimation());
                }
            }
        }

    }

    IEnumerator BlinkAnimation()
    {
        animator.SetBool("damage", true);

        yield return new WaitForSeconds(0.2f);

        animator.SetBool("damage", false);
    }

}
