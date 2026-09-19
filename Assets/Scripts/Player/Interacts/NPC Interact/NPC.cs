using System;
using UnityEngine;
using TMPro;
public class NPC : MonoBehaviour
{
    public GameObject interactText;

    private void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }
}
