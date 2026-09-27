using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public QuestObjectiveType objectiveType;
    [TextArea(1, 3)] public string description;
    public GameObject target;
    public Item item;
    public int requiredAmount = 1;
}