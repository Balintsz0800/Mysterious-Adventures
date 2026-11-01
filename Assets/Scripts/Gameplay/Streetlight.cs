using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Streetlight : MonoBehaviour
{
    [SerializeField] private GameObject Lamp;

    private void Start()
    {
        if (Lamp == null)
        {
            Lamp = GetComponentInChildren<Light2D>().gameObject;
        }
    }

    public void TurnOn()
    {
        Lamp.SetActive(true);
    }

    public void TurnOff()
    {
        Lamp.SetActive(false);
    }
}
