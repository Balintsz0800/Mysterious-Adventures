using UnityEngine;

public class NPCRelationships : MonoBehaviour
{
    [Header("NPC Relationships")]
    public int relationship;
    
    [SerializeField] private int minRelationship = -100;
    [SerializeField] private int maxRelationship = 100;

    public void ChangeRelationship(int amount)
    {
        relationship += amount;
        relationship = Mathf.Clamp(relationship, minRelationship, maxRelationship);
    }

    public void SetRelationship(int value)
    {
        relationship = value;
        relationship = Mathf.Clamp(value, minRelationship, maxRelationship);
    }
    
}