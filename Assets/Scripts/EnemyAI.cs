using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform enemyGFX;
    public Animator animator;
    public EnemyHealth enemyHealth;
    public PlayerController playerController;
    public float minDistance;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public float dist;
    public Vector2 direction;



    // public List <Sprite> enemySprites = new List<Sprite>();

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

        dist = Vector3.Distance(this.transform.position, playerController.transform.position);
    }

    private void Update()
    {

        animator.SetFloat("Horizontal", direction.normalized.x);
        animator.SetFloat("Vertical", direction.normalized.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        if (dist < minDistance)
        {
            if (direction.x > 0)
            {
                animator.SetBool("attackRight", true);
            }

            else if (direction.x < 0)
            {
                animator.SetBool("attackLeft", true);
            }

            else if (direction.y > 0)
            {
                animator.SetBool("attackUp", true);
            }

            else if (direction.y < 0)
            {
                animator.SetBool("attackDown", true);
            }
        }

        else
        {
            CancelAttackAnimations();
        }
        
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void FixedUpdate()
    {
        dist = Vector2.Distance(this.gameObject.transform.position, target.position);

        if (path == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

       

        Debug.Log(distance);
    }

    private void CancelAttackAnimations()
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackUp",   false);
        animator.SetBool("attackDown", false);
    }
 
    public void AttackDetector()
    {
        if(dist < minDistance && playerController.movement.x > 0 && this.direction.x > 0)
        {
            Debug.Log("Attack");
            enemyHealth.TakeDamage(1);
        }

        else if(dist < minDistance && playerController.movement.y > 0 && this.direction.y > 0)
        {
            Debug.Log("Attack");
            enemyHealth.TakeDamage(1);
        }
    }

}