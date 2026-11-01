using System;
using UnityEngine;

public class WorldItem : MonoBehaviour, Interactable
{
    public Item item;
    public int amount = 1;
    
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        InvManager invManager = FindFirstObjectByType<InvManager>();

        if (invManager.AddItem(item, amount))
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnItemCollected(item, amount);
                Destroy(gameObject);
            }

            Destroy(gameObject);
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
