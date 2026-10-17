using System;
using UnityEngine;

public class Animation : MonoBehaviour
{
    Animator animator;

    public float horizontal;
    public float vertical;
    public bool isMoving;
    public bool carrying;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
        animator.SetBool("isMoving", isMoving);
    }
}
