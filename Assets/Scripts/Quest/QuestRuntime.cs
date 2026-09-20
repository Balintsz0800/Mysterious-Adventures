using System.Collections.Generic;

[System.Serializable]
public class QuestRunTime
{
    public QuestData quest;
    public QuestState state;
    public List<int> progress = new List<int>();

    public QuestRunTime(QuestData quest)
    {
        this.quest = quest;
        state = QuestState.Active;
        
        foreach (QuestObjective objective in quest.objectives)
        {
            progress.Add(0);
        }
    }

    public bool IsComplate()
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

    public void Addprogress(int index, int amount)
    {
        if(index < 0 || index >= progress.Count)
        {
            return;
        }

        progress[index] += amount;

        if (progress[index] > quest.objectives[index].requiredAmount)
        {
            progress[index] = quest.objectives[index].requiredAmount;
        }
    }
}
