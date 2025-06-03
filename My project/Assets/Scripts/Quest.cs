using System.Collections.Generic;

[System.Serializable]// Gør klassen serialiserbar, så den kan bruges med JsonUtility
public class Quest// Repræsenterer en quest i spillet
{
    public bool isActive;//Indikerer om questen er aktiv
    public string Objective;//Beskrivelse af questens mål
    public int Reward;//Belønning for at fuldføre questen

    public List<QuestItemRequirement> requiredItems = new List<QuestItemRequirement>();// Liste over krav til items for at fuldføre questen
}

[System.Serializable]// Gør klassen serialiserbar, så den kan bruges med JsonUtility
public class QuestItemRequirement// Repræsenterer et krav om et item for at fuldføre en quest
{
    public int itemId;// Unik ID for itemet
    public string itemName;// Navn på itemet
    public int requiredAmount;// Antal af itemet, der kræves for at fuldføre questen
}