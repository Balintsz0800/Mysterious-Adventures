using System;
using UnityEngine;

public class Animation : MonoBehaviour
{
    Animator animator;

    public float horizontal;
    public float vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }
}
