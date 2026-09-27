using System.Collections.Generic;

[System.Serializable]
public class QuestRuntime
{
    public QuestData quest;
    public QuestState state;
    public List<int> progress = new List<int>();

    public QuestRuntime(QuestData quest)
    {
        this.quest = quest;
        state = QuestState.Active;

        foreach (QuestObjective objective in quest.objectives)
        {
            progress.Add(0);
        }
    }

    public bool IsComplete()
    {
        for (int i = 0; i < quest.objectives.Count; i++)
        {
            if (progress[i] < quest.objectives[i].requiredAmount)
            {
                return false;
            }
        }

        return true;
    }

    public void AddProgress(int i, int amount)
    {
        if (i < 0 || i >= progress.Count)
        {
            return;
        }

        progress[i] += amount;

        if (progress[i] > quest.objectives[i].requiredAmount)
        {
            progress[i] = quest.objectives[i].requiredAmount;
        }
    }
}