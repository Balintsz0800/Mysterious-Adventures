using System;
using UnityEngine;
using TMPro;
public class NPC : MonoBehaviour
{
    public GameObject interactText;
<<<<<<< Updated upstream
    NPCRelationships npcRelationships;
    public NPCDialogue npcDialogue;
=======
    [SerializeField] private NPCDialogue dialogue;
    public string NpcId;

    private NPCRelationships npcRelationships;
>>>>>>> Stashed changes

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