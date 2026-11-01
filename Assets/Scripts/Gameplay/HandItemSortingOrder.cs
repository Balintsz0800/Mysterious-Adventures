using System;
using UnityEngine;

public class HandItemSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Ysort playerY;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerY = GetComponentInParent<Ysort>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingLayerID = playerY.GetComponent<SpriteRenderer>().sortingLayerID;
        
        if (playerMovement.FacingDir.y > 0)
        {
            spriteRenderer.sortingOrder = playerY.SortingOrder -2;
        }
        else
        {
            spriteRenderer.sortingOrder = playerY.SortingOrder + 2;
        }
    }
}
