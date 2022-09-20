using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    private List <GameObject> targets = new List <GameObject>();
    public GameObject spawnpoint1;
    public GameObject spawnpoint2;
    public GameObject spawnpoint3;
    public GameObject spawnpoint4;
    public Vector2 movement;
    public BoxCollider2D collider;
    public AudioClip EnemyDamage;
    EnemyAI enemyAI;
    Vector2 size;


    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        size = collider.size;
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
            animator.SetTrigger("right");
        }

        else if (Input.GetMouseButtonDown(0) && movement.x < 0)
        {
            animator.SetTrigger("left");
        }

        else if (Input.GetMouseButtonDown(0) && movement.y > 0)
        {
            animator.SetTrigger("up");
        }

        else if (Input.GetMouseButtonDown(0) && movement.y < 0)
        {
            animator.SetTrigger("down");
        }

        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("down");
        }

        foreach (var target in targets)
        {
            if (target)
            {
                if (target.GetComponent<EnemyHealth>().killChecker() == true)
                {
                    targets.Remove(target);
                }
            }
        }
    }

    private void FixedUpdate()
    {
       rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void PlayerAttackDetector()
    {
        foreach (var target in targets)
        {
            if (target.GetComponentInChildren<EnemyAI>().dist < target.GetComponentInChildren<EnemyAI>().minHorizotalDistance)
            {
                if (this.movement.x > 0 && target.GetComponentInChildren<EnemyAI>().direction.x < 0 || this.movement.x < 0 && target.GetComponentInChildren<EnemyAI>().direction.x > 0)
                {
                    Debug.Log("attack");
                    target.GetComponentInChildren<EnemyHealth>().TakeDamage(1);
                    target.GetComponentInChildren<EnemyAI>().animator.SetTrigger("damage");
                    AudioSource.PlayClipAtPoint(EnemyDamage, Camera.main.transform.position);
                }
            }

            else if (target.GetComponentInChildren<EnemyAI>().dist < target.GetComponentInChildren<EnemyAI>().minVerticleDistance)
            {
                if (this.movement.y > 0 && target.GetComponentInChildren<EnemyAI>().direction.y < 0 || this.movement.y < 0 && target.GetComponentInChildren<EnemyAI>().direction.y > 0)
                {
                    Debug.Log("attack");
                    target.GetComponentInChildren<EnemyHealth>().TakeDamage(1);
                    target.GetComponentInChildren<EnemyAI>().animator.SetTrigger("damage");
                    AudioSource.PlayClipAtPoint(EnemyDamage, Camera.main.transform.position);
                }
            }
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            size = new Vector2(1.89315081f, 1.4193902f);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Walls"))
        {
            size = new Vector2(1.89315081f, 4.97681141f);;
        }
    }

    public void AddTarget( GameObject target)
    {
        targets.Add(target);
    }

}
