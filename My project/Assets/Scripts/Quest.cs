using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public string Objective;
    public int Reward;

    public List<QuestItemRequirement> requiredItems = new List<QuestItemRequirement>();
}

[System.Serializable]
public class QuestItemRequirement
{
    public int itemId;
    public string itemName;
    public int requiredAmount;
}