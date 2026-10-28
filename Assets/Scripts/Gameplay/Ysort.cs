using System;
using UnityEngine;

public class Ysort : MonoBehaviour
{
    [SerializeField] private Transform sortingPoint;
    [SerializeField] private int orderOffset;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sortingPoint == null)
        {
            sortingPoint = transform;
        }
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-sortingPoint.position.y * 100) + orderOffset;
    }
}
