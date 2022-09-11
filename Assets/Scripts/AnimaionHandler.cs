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

        animator.SetBool("backward", playerController.transform.position.y > transform.position.y);
        animator.SetBool("forward", playerController.transform.position.y < transform.position.y);
        animator.SetBool("right", playerController.transform.position.x > transform.position.x);
        animator.SetBool("left", playerController.transform.position.x < transform.position.x);

    }

    public void RunState()
    {
        this.animator.SetBool("forward", true);
    }

}