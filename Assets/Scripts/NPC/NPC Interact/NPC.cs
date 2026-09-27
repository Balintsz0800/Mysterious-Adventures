using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject interactText;
<<<<<<< HEAD
    [SerializeField] private NPCDialogue dialogue;

    private NPCRelationships npcRelationships;
=======
<<<<<<< Updated upstream
    NPCRelationships npcRelationships;
    public NPCDialogue npcDialogue;
=======
    [SerializeField] private NPCDialogue dialogue;
    public string NpcId;

    private NPCRelationships npcRelationships;
>>>>>>> Stashed changes
>>>>>>> npc

    private void Start()
    {
        npcRelationships = GetComponent<NPCRelationships>();

        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        if (dialogue == null)
        {
            dialogue = GetComponent<NPCDialogue>();
        }
    }

    public void Talk()
    {
        if (dialogue != null &&  dialogue.inInteractionRange)
        {
            dialogue.StartDialogue();
        }

        if (npcRelationships != null)
        {
            npcRelationships.ChangeRelationship(10);
        }

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnTalk(this);
        }
    }
}