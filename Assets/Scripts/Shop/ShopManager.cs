using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ShopItem
{
    public int cost;
    public int count;
    public Item item;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private Vector3 lastShopPosition;

    [SerializeField] public List<ShopItem> shopItems = new List<ShopItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RefreshShop();
    }

    private void FixedUpdate()
    {
        Vector3 playerPosition = Player.Instance.transform.position;

        if (Vector2.Distance(lastShopPosition, playerPosition) > 5f)
        {
            ShopUIManager.Instance.CloseShopUI();
        }
    }

    public void BuyItem(ShopItem item)
    {
        ShopItem shopItem = shopItems.Find(x => x == item);
        
        shopItem.count--;

        InvManager.instance.AddItem(item.item, 1);
        
        RefreshShop();
    }

    private void RefreshShop()
    {
        foreach (ShopItem item in shopItems.ToList())
        {
            if (item.count <= 0)
                shopItems.Remove(item);
        }

        ShopUIManager.Instance.BuildShopUI(shopItems);
    }

    public void SaveShopPosition(Vector3 position)
    {
        lastShopPosition = position;
    }
}
