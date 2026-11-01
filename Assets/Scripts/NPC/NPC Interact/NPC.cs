using UnityEngine;

public class NPC : MonoBehaviour, Interactable
{
    public GameObject interactText;
    [SerializeField] private NPCDialogue dialogue;
    public string npcID;
    private NPCRelationships npcRelationships;
    public NPCMovement movement;
    public SpriteRenderer spriteRenderer;
    
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

    public void Interact()
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

    public void Outline()
    {
        spriteRenderer.material = MaterialManager.Instance.outlineMaterial;
    }
    
    public void RemoveOutline()
    {
        spriteRenderer.material = MaterialManager.Instance.defaultMaterial;
    }
}
