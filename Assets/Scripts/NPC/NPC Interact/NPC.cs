using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject interactText;
    [SerializeField] private NPCDialogue dialogue;
    public string npcID;
    private NPCRelationships npcRelationships;
    public NPCMovement movement;

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
        if (dialogue != null && !dialogue.inInteractionRange)
        {
            return;
        }

        if (movement != null && movement.isWandering)
        {
            movement.StopMovement();
            movement.isWandering = false;
        }

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.TryDeliverItemsToNPC(this);
        }

        if (dialogue != null)
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