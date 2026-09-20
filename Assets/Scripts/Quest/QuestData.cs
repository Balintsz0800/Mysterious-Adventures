using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questName;
    [TextArea(3, 6)] public string description;
    public List<QuestObjective> objectives = new List<QuestObjective>();
    public int rewardMoney;
    public List<Item> rewardItems = new List<Item>();
}