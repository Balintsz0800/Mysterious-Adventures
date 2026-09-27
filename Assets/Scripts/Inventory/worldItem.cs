using UnityEngine;

public class worldItem : MonoBehaviour
{
    public Item item;
    public int amount = 1;

    public void Pickup()
    {
        InvManager invManager = FindObjectOfType<InvManager>();

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
}
