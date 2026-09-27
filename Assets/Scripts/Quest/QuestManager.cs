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
<<<<<<< Updated upstream
            
        }
    }
}
=======
            return;
        }

        activeQuests.Add(new QuestRuntime(quest));
        Debug.Log("Quest started: " + quest.questName);
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
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Talk)
                {
                    continue;
                }

                if (objective.targetID == npc.NpcId)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnItemCollected(Item item, int amount)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.CollectItem)
                {
                    continue;
                }

                if (objective.item == item)
                {
                    q.AddProgress(i, amount);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnLocationReached(string location)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Location)
                {
                    continue;
                }

                if (objective.targetID == location)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnInteract(string target)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Interact)
                {
                    continue;
                }

                if (objective.targetID == target)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnCraft(Item item)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Craft)
                {
                    continue;
                }

                if (objective.item == item)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnKill(string enemy)
    {
        foreach (QuestRuntime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectiveType != QuestObjectiveType.Kill)
                {
                    continue;
                }

                if (objective.targetID == enemy)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    private void CheckQuest(QuestRuntime q)
    {
        if (!q.IsComplete())
        {
            return;
        }

        q.state = QuestState.Completed;
        activeQuests.Remove(q);
        completedQuests.Add(q);

        Debug.Log("Quest completed: " + q.quest.questName);
    }
}
>>>>>>> Stashed changes
