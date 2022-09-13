using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;

    public void RunState()
    {
        animator.SetBool("movement", true);
    }
}
