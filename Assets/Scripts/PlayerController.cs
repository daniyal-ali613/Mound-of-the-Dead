using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Rigidbody2D rb;
    Vector2 movement;

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

        if (Input.GetMouseButtonDown(0) && movement.x < 0)
        {
            animator.SetBool("attackLeft", true);
        }

        if (Input.GetMouseButtonDown(0) && movement.y > 0)
        {
            animator.SetBool("attackUp", true);
        }

        if (Input.GetMouseButtonDown(0) && movement.y < 0)
        {
            animator.SetBool("attackDown", true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("attackDown", true);
        }

    }

    private void FixedUpdate()
    {
      rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void CancelAttackAnimation()
    {
        animator.SetBool("attackRight", false);
        animator.SetBool("attackLeft",  false);
        animator.SetBool("attackUp",    false);
        animator.SetBool("attackDown",  false);
    }

}
