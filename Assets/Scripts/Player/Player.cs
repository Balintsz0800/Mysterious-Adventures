using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Stamina")]
    public int maxStamina = 100;
    public int currentStamina;
    public int decreaseStamina = 1;


    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
}