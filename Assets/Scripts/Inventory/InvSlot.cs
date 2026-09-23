using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour, IDropHandler
{
    public Image img;
    public Color SelectedClr, NotSelectedClr;

    private void Awake()
    {
     Deselect();   
    }

    public void Select()
    {
        img.color = SelectedClr;
    }

    public void Deselect()
    {
        img.color = NotSelectedClr;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InvItem invItem = eventData.pointerDrag.GetComponent<InvItem>();
            invItem.parentAfterDrag = transform;
        }
    }
}