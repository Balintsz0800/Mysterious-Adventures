using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Stamina")]
    public float maxStamina = 100;
    public float currentStamina;


    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }
}