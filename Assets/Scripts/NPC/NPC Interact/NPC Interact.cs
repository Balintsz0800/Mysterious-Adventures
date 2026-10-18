using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] private float InteractDistance = 3f;
    [SerializeField] private LayerMask npcLayer;
    
    private NPC currentNpc = null;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerMovement == null)
        {
            return;
        }
        
        Vector2 facingDir = playerMovement.FacingDir;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDir, InteractDistance, npcLayer);

        NPC hitnpc = null;
        
        if (hit.collider != null)
        {
            hitnpc = hit.collider.GetComponent<NPC>();
        }

        if (currentNpc != hitnpc)
        {
            if (currentNpc != null && currentNpc.interactText != null)
            {
                currentNpc.interactText.SetActive(false);
            }
            
            currentNpc = hitnpc;

            if (currentNpc != null && currentNpc.interactText != null)
            {
                currentNpc.interactText.SetActive(true);
            }
        }

        if (currentNpc != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNpc.Talk();
        }
    }

    private void OnDisable()
    {
        if (currentNpc != null && currentNpc.interactText != null)
        {
            currentNpc.interactText.SetActive(false);
        }
        
        currentNpc = null;
    }
}