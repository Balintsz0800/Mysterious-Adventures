using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestRunTime> activeQuests = new List<QuestRunTime>();
    public List<QuestRunTime> complateQuests = new List<QuestRunTime>();

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

        activeQuests.Add(new QuestRunTime(quest));
        Debug.Log("Quest started: " + quest.questName);
    }
   public QuestState GetQuestState(QuestData quest)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            if (q.quest == quest)
            {
                return q.state;
            }
        }

        foreach (QuestRunTime q in complateQuests)
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
            foreach (QuestRunTime q in activeQuests)
            {
                for (int i = 0; i < q.quest.objectives.Count; i++)
                {
                QuestObjective objective = q.quest.objectives[i];
                if (objective.objectivetype != QuestObjectiveType.Talk)
                {
                    continue;
                }

                if (objective.target == npc.gameObject)
                {
                    q.Addprogress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }

    public void OnItemCollected(Item item, int amount)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectivetype != QuestObjectiveType.CollectItem)
                {
                    continue;
                }

                if (objective.item == item)
                {
                    q.Addprogress(i, amount);
                    CheckQuest(q);
                }
            }
        }
    }
    public void OnLocationReached(GameObject location)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectivetype != QuestObjectiveType.Location)
                {
                    continue;
                }

                if (objective.target == location)
                {
                    q.Addprogress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }
    public void OnInteract(GameObject target)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectivetype != QuestObjectiveType.interact)
                {
                    continue;
                }

                if (objective.target == target)
                {
                    q.Addprogress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }
     public void OnCraft(Item item)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectivetype != QuestObjectiveType.Craft)
                {
                    continue;
                }

                if (objective.item == item)
                {
                    q.Addprogress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }
    public void OnKill(GameObject enemy)
    {
        foreach (QuestRunTime q in activeQuests)
        {
            for (int i = 0; i < q.quest.objectives.Count; i++)
            {
                QuestObjective objective = q.quest.objectives[i];

                if (objective.objectivetype != QuestObjectiveType.Kill)
                {
                    continue;
                }

                if (objective.target == enemy)
                {
                    q.Addprogress(i, 1);
                    CheckQuest(q);
                }
            }
        }
    }
    private void CheckQuest(QuestRunTime q)
    {
        if (!q.IsComplate())
        {
            return;
        }

        q.state = QuestState.Complated;
        activeQuests.Remove(q);
        complateQuests.Add(q);

        Debug.Log("Quest completed: " + q.quest.questName);
    }

}