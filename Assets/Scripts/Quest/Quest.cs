using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Quest
{
    public int questID;
    public int questName;
    
    [TextArea(2,5)] public string questDescription;
    
    [Header("Requirements")]
    public List<QuestRequirements> requirements = new List<QuestRequirements>();

    [Header("Rewards")]
    public int rewardMoney;

    public bool isComplete()
    {
        foreach (QuestRequirements req in requirements)
        {
            if (!req.isComplete())
            {
                return false;
            }
        }
        return true;
    }

    public void AddItem(Item item, int amount)
    {
        foreach (QuestRequirements req in requirements)
        {
            if (req.item == item)
            {
                req.currentAmount += amount;

                if (req.currentAmount > req.requiredAmount)
                {
                    req.currentAmount = req.requiredAmount;
                }
                break;
            }
        }
    }
}
