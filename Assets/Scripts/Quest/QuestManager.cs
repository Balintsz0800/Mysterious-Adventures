using System;
using UnityEngine;
using UnityEngine.Rendering;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    
    public Quest currentQuest;

    private void Awake()
    {
        instance = this;
    }
    
    void StartQuest(Quest quest)
    {
        currentQuest = quest;
    }

    void AddItem(Item item, int amount)
    {
        if (currentQuest == null)
        {
            return;
        }
        
        currentQuest.AddItem(item, amount);

        if (currentQuest.isComplete())
        {
            
        }
    }
}
