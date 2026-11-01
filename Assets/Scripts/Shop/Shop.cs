using UnityEngine;

public class Shop : MonoBehaviour, Interactable
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Interact()
    {
        ShopManager.Instance.SaveShopPosition(transform.position);
        ShopUIManager.Instance.OpenShopUI();
    }
    
    public void Outline()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.material = MaterialManager.Instance.outlineMaterial;
    }

    public void RemoveOutline()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.material = MaterialManager.Instance.defaultMaterial;
    }
}
