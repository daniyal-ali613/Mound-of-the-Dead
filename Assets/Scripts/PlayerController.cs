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
    public AudioClip EnemyDamage;
    public AudioClip AxeSwing;
    private BoxCollider2D box;
    Vector2 currentOffset;
    EnemyAI enemyAI;


    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        box = GetComponent<BoxCollider2D>();
        currentOffset = box.offset;
    }
    void Update()
    {

        if(Time.timeScale == 1)
        {

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);


            if (Input.GetMouseButtonDown(0) && movement.x > 0)
            {
                animator.SetTrigger("right");
                AudioSource.PlayClipAtPoint(AxeSwing, Camera.main.transform.position, 0.1f);
            }

            else if (Input.GetMouseButtonDown(0) && movement.x < 0)
            {
                animator.SetTrigger("left");
                AudioSource.PlayClipAtPoint(AxeSwing, Camera.main.transform.position, 0.1f);

            }

            else if (Input.GetMouseButtonDown(0) && movement.y > 0)
            {
                animator.SetTrigger("up");
                AudioSource.PlayClipAtPoint(AxeSwing, Camera.main.transform.position, 0.1f);

            }

            else if (Input.GetMouseButtonDown(0) && movement.y < 0)
            {
                animator.SetTrigger("down");
                AudioSource.PlayClipAtPoint(AxeSwing, Camera.main.transform.position, 0.1f);

            }

            else if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("down");
                AudioSource.PlayClipAtPoint(AxeSwing, Camera.main.transform.position, 0.1f);

            }

            foreach (var target in targets)
            {
                if (target)
                {
                    if (target.GetComponentInChildren<EnemyHealth>().killChecker() == true)
                    {
                        target.gameObject.SetActive(false);
                    }
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
            if (target)
            {
                if (target.GetComponentInChildren<EnemyAI>().GetDist() < target.GetComponentInChildren<EnemyAI>().maxDistance)
                {
                        target.GetComponentInChildren<EnemyHealth>().TakeDamage(1);
                        target.GetComponentInChildren<EnemyAI>().animator.SetTrigger("damage");
                        AudioSource.PlayClipAtPoint(EnemyDamage, Camera.main.transform.position,0.1f);
                }
            }

            else
            {
                return;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (this.movement.y < 0 && box != null || this.movement.x == 0 && this.movement.y == 0 && box != null)
            {
                box.enabled = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            box.enabled = true;
        }
    }

    public void AddTarget( GameObject target)
    {
        targets.Add(target);
    }

}
