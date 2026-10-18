using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public GameObject questTextBox;
    public TMP_Text questTitle;
    public TMP_Text questState;
    
    public List<QuestRuntime> activeQuests = new List<QuestRuntime>();
    public List<QuestRuntime> completedQuests = new List<QuestRuntime>();

    private void Start()
    {
        questTextBox.SetActive(false);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartQuest(QuestData quest)
    {
        if (quest == null || GetQuestState(quest) != QuestState.NotStarted)
        {
            return;
        }

        QuestRuntime runtime = new  QuestRuntime(quest);
        activeQuests.Add(runtime);
        CheckHaveItemObjectives();

        if (activeQuests.Contains(runtime) && questTextBox != null)
        {
            questTextBox.SetActive(true);

            if (questTitle != null)
            {
                questTitle.text = "Quest started: " + quest.questName;
            }
        }
    }

    public QuestState GetQuestState(QuestData quest)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            if (q.quest == quest)
            {
                return q.state;
            }
        }

        foreach (QuestRuntime q in completedQuests)
        {
            if (q.quest == quest)
            {
                return q.state;
            }
        }

        return QuestState.NotStarted;
    }

    public void OnTalk(NPC npc)
    {
        if (npc == null)
        {
            return;
        }
        
        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType == QuestObjectiveType.Talk && objective.targetID == npc.npcID)
                {
                    runtime.AddProgress(i, 1);
                }
            }
            CheckQuest(runtime);
        }
        CheckHaveItemObjectives();
    }

    public void OnItemCollected(Item item, int amount)
    {
        if (item == null || amount <= 0)
        {
            return;
        }

        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType == QuestObjectiveType.CollectItem && objective.item == item)
                {
                    runtime.AddProgress(i, amount);
                }
            }
            CheckQuest(runtime);
        }

        CheckHaveItemObjectives();
    }

    private void CheckHaveItemObjectives()
    {
        if (InvManager.instance == null)
        {
            return;
        }
        
        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType != QuestObjectiveType.HaveItem || objective.item == null)
                {
                    continue;
                }

                int inventoryAmount = CountItemInInventory(objective.item);

                if (inventoryAmount >= objective.requiredAmount)
                {
                    runtime.AddProgress(i, objective.requiredAmount);
                }
            }
            CheckQuest(runtime);
        }
    }

    public int CountItemInInventory(Item item)
    {
        if (InvManager.instance == null || item == null)
        {
            return 0;
        }

        int total = 0;

        foreach (InvSlot slot in InvManager.instance.slots)
        {
            if (slot == null)
            {
                continue;
            }

            InvItem invItem = slot.GetComponentInChildren<InvItem>();

            if (invItem != null && invItem.item == item)
            {
                total += invItem.count;
            }
        }
        return total;
    }

    public bool TryDeliverItemsToNPC(NPC npc)
    {
        if (npc == null || InvManager.instance == null)
        {
            return false;
        }

        bool deliveredAny = false;
        
        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType != QuestObjectiveType.DeliverItem || objective.targetID != npc.npcID || objective.item == null)
                {
                    continue;
                }

                int needed = objective.requiredAmount - runtime.progress[i];

                if (needed <= 0)
                {
                    continue;
                }

                int available = CountItemInInventory(objective.item);
                int amountToRemove = Mathf.Min(needed, available);

                if (amountToRemove <= 0)
                {
                    continue;
                }
                
                int removed = RemoveItemFromInventory(objective.item, amountToRemove);

                if (removed > 0)
                {
                    runtime.AddProgress(i, removed);
                    deliveredAny = true;
                }
            }
            CheckQuest(runtime);
        }

        if (deliveredAny)
        {
            CheckHaveItemObjectives();
        }
        
        return deliveredAny;
    }

    private int RemoveItemFromInventory(Item item, int amount)
    {
        if (InvManager.instance == null || item == null || amount <= 0)
        {
            return 0;
        }
        
        int remaining = amount;
        InvManager inv = InvManager.instance;

        foreach (InvSlot slot in inv.slots)
        {
            if (slot == null || remaining <= 0)
            {
                continue;
            }

            InvItem invItem = slot.GetComponentInChildren<InvItem>();

            if (invItem == null || invItem.item != item)
            {
                continue;
            }
            
            int removed = Mathf.Min(invItem.count, remaining);

            invItem.count -= removed;
            remaining -= removed;

            if (invItem.count <= 0)
            {
                Destroy(invItem.gameObject);
            }
            else
            {
                invItem.RefreshCount();
            }
        }

        return amount - remaining;
    }

    public void OnLocationReached(string location)
    {
        if (string.IsNullOrEmpty(location))
        {
            return;
        }

        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];

            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Location)
                {
                    continue;
                }

                if (objective.targetID == location)
                {
                    runtime.AddProgress(i, 1);
                }
            }

            CheckQuest(runtime);
        }
    }

    private void UpdateTargetObjective(QuestObjectiveType type, string targetID)
    {
        if (string.IsNullOrEmpty(targetID))
        {
            return;
        }

        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType == type && objective.targetID == targetID)
                {
                    runtime.AddProgress(i, 1);
                }
            }
            CheckQuest(runtime);
        }
    }

    public void OnInteract(string target)
    {
        if (string.IsNullOrEmpty(target))
        {
            return;
        }

        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];

            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Interact)
                {
                    continue;
                }

                if (objective.targetID == target)
                {
                    runtime.AddProgress(i, 1);
                }
            }

            CheckQuest(runtime);
        }
    }

    public void OnCraft(Item item)
    {
        if (item == null)
        {
            return;
        }
        
        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];
            
            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];
            
                if (objective.objectiveType == QuestObjectiveType.Craft && objective.item == item)
                {
                    runtime.AddProgress(i, 1);
                }
            }
            CheckQuest(runtime);
        }
        CheckHaveItemObjectives();
    }

    public void OnKill(string enemy)
    {
        if (string.IsNullOrEmpty(enemy))
        {
            return;
        }

        for (int q = activeQuests.Count - 1; q >= 0; q--)
        {
            QuestRuntime runtime = activeQuests[q];

            for (int i = 0; i < runtime.quest.objectives.Count; i++)
            {
                QuestObjective objective = runtime.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Kill)
                {
                    continue;
                }

                if (objective.targetID == enemy)
                {
                    runtime.AddProgress(i, 1);
                }
            }

            CheckQuest(runtime);
        }
    }

    private void CheckQuest(QuestRuntime runtime)
    {
        if (runtime == null || !runtime.IsComplete())
        {
            return;
        }

        runtime.state = QuestState.Completed;

        if (!activeQuests.Remove(runtime))
        {
           return;
        }

        if (!completedQuests.Contains(runtime))
        {
            completedQuests.Add(runtime);
        }

        if (questTextBox != null)
        {
            questTextBox.SetActive(true);
        }

        if (questTitle != null)
        {
            questTitle.text = "Quest completed: " +  runtime.quest.name;
        }
    }
}