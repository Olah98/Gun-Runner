using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IsRunning()
    {
        animator.SetBool("isIdling", false);
        animator.SetBool("isDying", false);
        animator.SetBool("isRunning", true);

    }

    public void IsIdling()
    {
        animator.SetBool("isIdling", true);
        animator.SetBool("isDying", false);
        animator.SetBool("isRunning", false);
    }

    public void DeathAnimator()
    {
        animator.SetBool("isIdling", false);
        animator.SetBool("isDying", true);
        animator.SetBool("isRunning", false);
    }
}
