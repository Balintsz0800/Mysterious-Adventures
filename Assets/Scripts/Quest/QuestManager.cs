using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestRuntime> activeQuests = new List<QuestRuntime>();
    public List<QuestRuntime> completedQuests = new List<QuestRuntime>();

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
        if (quest == null)
        {
            return;
        }

        if (GetQuestState(quest) != QuestState.NotStarted)
        {
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

                if (objective.target == npc.gameObject)
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

    public void OnLocationReached(GameObject location)
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

                if (objective.target == location)
                {
                    q.AddProgress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnInteract(GameObject target)
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

                if (objective.target == target)
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

    public void OnKill(GameObject enemy)
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

                if (objective.target == enemy)
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