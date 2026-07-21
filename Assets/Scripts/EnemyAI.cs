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
    float dist, distance;

    [Header("Formation / Separation")]
    public float separationRadius = 0.5f;   // KEEP THIS SMALLER than the gap between your slots -
                                            // otherwise correctly-spaced neighbors keep shoving each
                                            // other and it looks like fighting even though slots are fine
    public float separationStrength = 15f;  // lowered - this used to be strong enough to fight the seek force
    public float maxSpeed = 3f;             // hard clamp so forces can't compound into jitter
    public LayerMask enemyLayer;            // set this to the layer your enemies are on
    private int mySlot = -1;
    private Vector2 slotTarget;

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void OnEnable()
    {
        // If this enemy is being reused from a pool (disabled then re-enabled
        // rather than freshly instantiated), Start() won't run again, so make
        // sure it re-requests a slot here too.
        if (mySlot == -1 && FormationManager.Instance != null)
        {
            mySlot = FormationManager.Instance.RequestSlot(this);
        }
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

        dist = Vector3.Distance(this.transform.position, playerController.transform.position);
        attack = false;
        currentSpeed = speed;
        speed = 0;

        if (FormationManager.Instance != null)
        {
            mySlot = FormationManager.Instance.RequestSlot(this);
        }
    }

    void UpdatePath()
    {
        // Figure out where THIS enemy should path to. If it has a formation
        // slot, use that instead of the player's exact position so enemies
        // spread out around the player instead of stacking on top of them.
        Vector2 destination = target.position;

        if (FormationManager.Instance != null && mySlot != -1)
        {
            slotTarget = FormationManager.Instance.GetSlotPosition(mySlot);
            destination = slotTarget;
        }

        if (seeker.IsDone())
            seeker.StartPath(rb.position, destination, OnPathComplete);
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

        // --- Separation from other enemies so they don't clump/flicker ---
        Vector2 separation = GetSeparationForce();
        if (separation != Vector2.zero)
        {
            rb.AddForce(separation * separationStrength);
        }

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

        // Hard clamp - without this, seek force + separation force can compound
        // frame over frame with no drag to bleed it off, which is what "fighting"
        // jitter usually is in practice.
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    // Simple avoidance: push away from any enemy that's too close.
    // Assign your enemies to a layer and set enemyLayer in the Inspector for this to work.
    private Vector2 GetSeparationForce()
    {
        Vector2 push = Vector2.zero;
        Collider2D[] nearby = Physics2D.OverlapCircleAll(rb.position, separationRadius, enemyLayer);

        foreach (var col in nearby)
        {
            if (col.attachedRigidbody == rb) continue; // skip self

            Vector2 away = rb.position - (Vector2)col.transform.position;
            float dist = away.magnitude;
            if (dist > 0.01f)
            {
                push += away.normalized / dist; // closer enemies push harder
            }
        }

        return push;
    }

    private void Update()
    {
        // Below this speed, the enemy is basically standing still in its slot.
        // Using smoothedVelocity's sign for facing here is what causes the
        // front/back flicker - separation force creates tiny back-and-forth
        // nudges even at rest, and their sign flips constantly. Facing the
        // player instead is stable because it doesn't depend on velocity at all.
        bool isStationary = smoothedVelocity.magnitude < 0.3f;

        if (isStationary)
        {
            Vector2 toPlayer = ((Vector2)target.position - rb.position).normalized;
            animator.SetFloat("Horizontal", toPlayer.x);
            animator.SetFloat("Vertical", toPlayer.y);
        }
        else
        {
            animator.SetFloat("Horizontal", smoothedVelocity.normalized.x);
            animator.SetFloat("Vertical", smoothedVelocity.normalized.y);
        }

        if (playerHealth.die == false)
        {
            bool canAttack = dist <= maxDistance;

            // Only actually enter attack state if the formation manager
            // grants this enemy a turn. Otherwise it just stands in its
            // slot (still tracked, still facing the player) and waits.
            if (canAttack && FormationManager.Instance != null)
            {
                canAttack = FormationManager.Instance.RequestAttackTurn(this);
            }

            if (canAttack)
            {
                animator.SetBool("Idle", false);

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

                // Waiting for a turn (or just holding formation, out of range)
                // and not actually moving right now -> proper Idle state,
                // instead of sitting in a directional walk/attack-facing pose.
                animator.SetBool("Idle", isStationary);

                if (FormationManager.Instance != null)
                {
                    FormationManager.Instance.EndAttackTurn(this);
                }
            }
        }
        else
        {
            attack = false;
            CancelAttackAnimations();
            animator.SetBool("Idle", true);
            if (FormationManager.Instance != null)
            {
                FormationManager.Instance.EndAttackTurn(this);
            }
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
        else if (playerController.movement.y < 0 && attack == true)
        {
            playerController.animator.SetTrigger("front");
        }
        else if (playerController.movement.y > 0 && attack == true)
        {
            playerController.animator.SetTrigger("front");
        }
        else if (playerController.movement.x == 0 && playerController.movement.y == 0 && attack == true)
        {
            playerController.animator.SetTrigger("front");
        }
    }

    private void CancelAttackAnimations()
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft", false);
        animator.SetBool("attackUp", false);
        animator.SetBool("attackDown", false);
    }

    public void EnemyAttackDetector()
    {
        if (attack == true)
        {
            AudioSource.PlayClipAtPoint(enemyAttackSound, transform.position, 0.1f);
            playerHealth.TakeDamage(2);
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
            // NOTE: this used to force rb.bodyType = Static here. That's what
            // was causing enemies to freeze permanently and block the rest of
            // the crowd - a Static body never unfreezes unless OnCollisionExit2D
            // fires, and in a packed crowd it often never does. Staying Dynamic
            // and just clamping velocity keeps the same "plant your feet while
            // attacking" feel without ever creating an unmovable wall.
            rb.velocity = Vector2.zero;

            if (playerController.movement.y < 0 && this.direction.y > 0)
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

    // OnDisable covers BOTH real Destroy() calls and the common "death = SetActive(false)"
    // pattern (e.g. object pooling). If your death code only disables the object,
    // OnDestroy alone would never fire and this enemy's slot / attack-turn would
    // stay occupied forever - which is exactly what was blocking attacks after a
    // few deaths.
    private void OnDisable()
    {
        if (FormationManager.Instance != null)
        {
            if (mySlot != -1)
            {
                FormationManager.Instance.ReleaseSlot(mySlot);
                mySlot = -1;
            }
            FormationManager.Instance.EndAttackTurn(this);
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