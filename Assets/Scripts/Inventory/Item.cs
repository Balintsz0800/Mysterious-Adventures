using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public GameObject itemPrefab;
    public GameObject handPrefab;
    public int durability;
    
    public Sprite image;

    public enum ItemType
    {
        Pickaxe,
        Axe,
        Sword,
    }
}
