using System;
using UnityEngine;
using TMPro;
public class NPC : MonoBehaviour
{
    public GameObject interactText;
    NPCRelationships npcRelationships;
    public NPCDialogue npcDialogue;

    private void Start()
    {
        npcRelationships = GetComponent<NPCRelationships>();
        
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    public void Talk()
    {
        if (npcDialogue != null)
        {
            npcDialogue.StartDialogue();
        }
    }
}