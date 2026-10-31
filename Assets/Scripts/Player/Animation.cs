using System;
using UnityEngine;

public class Animation : MonoBehaviour
{
    Animator animator;

    public float horizontal;
    public float vertical;
    public bool isMoving;
    public bool carrying;

    [SerializeField] private RuntimeAnimatorController run;
    [SerializeField] private RuntimeAnimatorController carry;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!carrying)
        {
            animator.runtimeAnimatorController = run;
        }
        else
        {
            animator.runtimeAnimatorController = carry;

            animator.SetBool("carrying", carrying);
        }

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("isMoving", isMoving);
    }
}
