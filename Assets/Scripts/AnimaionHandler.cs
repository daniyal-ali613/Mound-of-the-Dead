using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaionHandler : MonoBehaviour
{
    public PlayerController playerController;
    Animator animator;


    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    void Start()
    {
        this.animator.SetBool("forward", false);
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
            this.animator.SetBool("backward", true);
            this.animator.SetBool("right", false);
            this.animator.SetBool("left", false);
        }

        if (playerController.transform.position.y < this.transform.position.y)
        {
            Debug.Log("Down");

            this.animator.SetBool("backward", false);
            this.animator.SetBool("right", false);
            this.animator.SetBool("left", false);
        }


        if (playerController.transform.position.x > this.transform.position.x)
        {
            Debug.Log("Right");

            this.animator.SetBool("right", true);
            this.animator.SetBool("backward", false);
            this.animator.SetBool("left", false);

        }

        if (playerController.transform.position.x < this.transform.position.x)
        {
            Debug.Log("Left");

            this.animator.SetBool("left", true);
            this.animator.SetBool("backward", false);
            this.animator.SetBool("right", false);
        }
    }

    public void RunState()
    {
        this.animator.SetBool("forward", true);
    }

}