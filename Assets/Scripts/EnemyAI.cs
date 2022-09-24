using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform enemyGFX;
    public Animator animator;
    public PlayerController playerController;
    public float maxDistance;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;
    public Vector2 direction;
    private Vector2 smoothedVelocity;
    public SpriteRenderer sprite;
    public PlayerHealth playerHealth;
    public AudioClip enemyAttackSound;
    public SpriteRenderer playerRenderer;
    float currentSpeed;
    public float smoothedSpeed;
    bool attack;
    float dist,distance;




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
        attack = false;
        currentSpeed = speed;
        speed = 0;
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

        smoothedVelocity = Vector2.MoveTowards(smoothedVelocity, rb.velocity, smoothedSpeed);

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

        distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

    }

    private void Update()
    {

        if (smoothedVelocity.magnitude < 0.1f)
        {
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }
        else
        {
            animator.SetFloat("Horizontal", smoothedVelocity.normalized.x);
            animator.SetFloat("Vertical",   smoothedVelocity.normalized.y);
        }


        if (playerHealth.die == false)
        {
            if (dist <= maxDistance)
            {
                if (direction.x > 0 && rb.velocity.y <= 0 && rb.velocity.y <= 0)
                {
                    animator.SetBool("attackRight", true);
                    attack = true;
                }

                if (direction.x < 0 && rb.velocity.y <= 0 && rb.velocity.x <= 0)
                {
                    animator.SetBool("attackLeft", true);
                    attack = true;
                }

                if (direction.y > 0 && rb.velocity.x <= 0 && rb.velocity.y <= 0)
                {
                    animator.SetBool("attackUp", true);
                    attack = true;
                }

                if (direction.y < 0 && rb.velocity.x <= 0 && rb.velocity.y <= 0)
                {
                    animator.SetBool("attackDown", true);
                    attack = true;
                }
              
            }

            else
            {
                attack = false;
                CancelAttackAnimations();
            }
        }

        else
        {
            attack = false;
            CancelAttackAnimations();
            animator.SetBool("Idle", true);
        }


        DamageAnimation();
    }

    private void DamageAnimation()
    {

        if (playerController.movement.x > 0 && attack == true)
        {
            playerController.animator.SetTrigger("side");
        }

        else if (playerController.movement.x < 0 && attack == true)
        {
            playerController.animator.SetTrigger("sideLeft");
        }

        else if (playerController.movement.y > 0 && attack == true)
        {
            playerController.animator.SetTrigger("front");
        }

        else if (playerController.movement.y < 0 && attack == true)
        {
            playerController.animator.SetTrigger("front");
        }

        else if (attack == true)
        {
            playerController.animator.SetTrigger("front");
        }
    }


    private void CancelAttackAnimations()
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackUp",   false);
        animator.SetBool("attackDown", false);
    }
 
    public void EnemyAttackDetector()
    {
        if(attack == true )
        {
            AudioSource.PlayClipAtPoint(enemyAttackSound,Camera.main.transform.position);
            playerHealth.TakeDamage(1);
        }
    }

    public void RunState()
    {
        animator.SetBool("movement", true);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Static;


            if (playerController.movement.y < 0 &&  this.direction.y > 0)
            {
                playerRenderer.sortingOrder = 4;
            }

            if (playerController.movement.y == 0 && this.direction.y > 0)
            {
                playerRenderer.sortingOrder = 2;
            }

            if (playerController.movement.y == 0 && this.direction.y < 0)
            {
                playerRenderer.sortingOrder = 4;
            }


             if (playerController.movement.y > 0 && this.direction.y < 0)
            {
                playerRenderer.sortingOrder = 4;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void SpeedBooster()
    {
        speed = currentSpeed;
    }

    public float GetDist()
    {
        return dist;
    }

}