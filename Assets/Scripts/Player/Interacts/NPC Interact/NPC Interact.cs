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

        if (playerMovement == null)
        {
            return;
        }
    }

    void Update()
    {
        Vector2 facingDir = playerMovement.FacingDir;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDir, InteractDistance, npcLayer);
        
        
        if (hit.collider != null)
        {
            currentNpc = hit.collider.gameObject.GetComponent<NPC>();

            if (currentNpc != null)
            {
                currentNpc.interactText.SetActive(true);
            }
        }
        else
        {
            if (currentNpc != null)
            {
                currentNpc.interactText.SetActive(false);
                currentNpc = null;
            }
        }
    }
}
