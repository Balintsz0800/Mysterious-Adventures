using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public QuestObjectiveType objectiveType;
    [TextArea(1, 3)] public string description;
<<<<<<< HEAD
    public GameObject target;
=======
    public string targetID;
>>>>>>> npc
    public Item item;
    public int requiredAmount = 1;
}