using System;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public int amount = 1;
    
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material outlineMaterial;
    
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Pickup()
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

    public void OutlineItem()
    {
        spriteRenderer.material = outlineMaterial;
    }
    
    public void RemoveOutline()
    {
        spriteRenderer.material = defaultMaterial;
    }
}
