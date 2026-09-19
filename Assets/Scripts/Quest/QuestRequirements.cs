using UnityEngine;

[System.Serializable] public class QuestRequirements
{
    public Item item;
    
    public int requiredAmount;
    public int currentAmount;

    public bool isComplete()
    {
        return currentAmount >= requiredAmount;
    }
}
