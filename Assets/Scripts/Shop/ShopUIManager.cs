using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopHolder;
    public Transform shopItemHolder;
    public GameObject shopItemUIPrefab;
    
    public static ShopUIManager Instance;

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
        CloseShopUI();
    }

    public void BuildShopUI(List<ShopItem> items)
    {
        for (int i = 0; i < shopItemHolder.childCount; i++)
        {
            Destroy(shopItemHolder.GetChild(i).gameObject);
        }

        foreach (ShopItem item in items)
        {
            ShopItemUI shopItem = Instantiate(shopItemUIPrefab, shopItemHolder).GetComponent<ShopItemUI>();
            
            shopItem.SetItem(item);
        }
    }

    public void OpenShopUI()
    {
        shopHolder.SetActive(true);
    }

    public void CloseShopUI()
    {
        shopHolder.SetActive(false);
    }
}