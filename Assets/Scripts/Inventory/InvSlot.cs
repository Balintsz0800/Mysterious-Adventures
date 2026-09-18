using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
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
}
