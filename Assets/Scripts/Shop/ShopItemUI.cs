using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public TMP_Text itemCount;
    public Image itemImage;
    
    [NonSerialized] public ShopItem item;

    public void SetItem(ShopItem shopItem)
    {
        this.item = shopItem;

        itemName.text = shopItem.item.name;
        itemPrice.text = shopItem.cost.ToString();
        itemCount.text = shopItem.count.ToString();
        itemImage.sprite = shopItem.item.image;
    }

    public void BuyItem()
    {
        ShopManager.Instance.BuyItem(item);
    }
}