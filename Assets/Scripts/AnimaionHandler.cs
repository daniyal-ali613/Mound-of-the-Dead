using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaionHandler : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    void Start()
    {
        animator.SetBool("forward", false);
    }

    void Update()
    {
        EnemyAnimationState();
    }

    private void EnemyAnimationState()
    {

        if (playerController.transform.position.y > this.transform.position.y)
        {
            Debug.Log("Up");
            animator.SetBool("backward", true);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
        }

        if (playerController.transform.position.y < this.transform.position.y)
        {
            Debug.Log("Down");

            animator.SetBool("backward", false);
            animator.SetBool("right", false);
            animator.SetBool("left", false);
        }


        if (playerController.transform.position.x > this.transform.position.x)
        {
            Debug.Log("Right");

            animator.SetBool("right", true);
            animator.SetBool("backward", false);
            animator.SetBool("left", false);

        }

        if (playerController.transform.position.x < this.transform.position.x)
        {
            Debug.Log("Left");

            animator.SetBool("left", true);
            animator.SetBool("backward", false);
            animator.SetBool("right", false);
        }
    }

    public void RunState()
    {
        animator.SetBool("forward", true);
    }

}