using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class QuestGiver : MonoBehaviour, IInteractable// IInteractable interface for interaction functionality
{
    public Quest quest;// Reference to the current quest being given by the QuestGiver
    public DataFetcher dataFetcher;// Reference to the DataFetcher to get item data for quests
    public TMP_Text questUIText;// UI text element to display the quest objective
    public PlayerStats playerStats;// Reference to PlayerStats to manage player inventory and stats

    void Start()
    {
        quest = null;
        // Automatisk find PlayerStats hvis ikke sat
        if (playerStats == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");// Finder spilleren i scenen ved hjælp af tagget "Player"
            if (playerObj != null)
            {
                playerStats = playerObj.GetComponent<PlayerStats>();// Henter PlayerStats komponenten fra spilleren
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with QuestGiver");
        if (quest == null)// Tjekker om der ikke er nogen aktiv quest
        {
            GenerateQuest();// Genererer en ny quest hvis ingen aktiv
        }
        else if (quest.isActive && CheckIfQuestCompleted())// Tjekker om questen er aktiv og om den er færdiggjort
        {
            CompleteQuest();// Fuldfører questen hvis den er færdiggjort
        }
        else
        {
            Debug.Log("Quest aktiv men endnu ikke færdiggjort.");
            ShowQuestInUI(); // Vis den aktive quest igen
        }
    }

    private void GenerateQuest()// Genererer en ny quest
    {
        Debug.Log("Forsøger at generere ny quest...");

        if (dataFetcher == null || dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)//   Tjekker om DataFetcher og fetchedItems er klar
        {
            Debug.LogWarning("DataFetcher eller fetchedItems er ikke klar.");
            return;
        }

        quest = new Quest();// Opretter en ny Quest instans
        quest.isActive = true;// Sætter questen som aktiv
        quest.Reward = Random.Range(150, 250);// Tildeler en tilfældig belønning mellem 150 og 250

        var shuffledItems = dataFetcher.fetchedItems.OrderBy(x => Random.value).ToList();// Blander listen af items fra DataFetcher for at vælge tilfældige items til questen
        int itemCount = Random.Range(1, 4);// Vælger et tilfældigt antal items mellem 1 og 3

        for (int i = 0; i < itemCount; i++) // Gennemgår det tilfældigt valgte antal items
        {
            var item = shuffledItems[i];// Henter det i'te item fra den blandede liste
            int requiredAmount = Random.Range(1, 4);// Tildeler et tilfældigt antal krævede items mellem 1 og 3

            quest.requiredItems.Add(new QuestItemRequirement// Opretter en ny QuestItemRequirement for det valgte item
            {
                itemId = item.ItemID,// Sætter item ID
                itemName = item.Name,// Sætter item navn
                requiredAmount = requiredAmount// Sætter det krævede antal
            });
        }

        string objectiveText = "Saml: ";// Initialiserer objective tekst
        foreach (var req in quest.requiredItems)// Gennemgår alle krav i questen
        {
            objectiveText += $"{req.requiredAmount}x {req.itemName}, ";// Tilføjer krav til objective tekst
        }

        quest.Objective = objectiveText.TrimEnd(',', ' ');// Fjerner det sidste komma og mellemrum fra objective teksten

        Debug.Log("Quest genereret: " + quest.Objective);
        ShowQuestInUI();// Viser den genererede quest i UI'en
    }

    private void ShowQuestInUI()// Viser den aktive quest i UI'en
    {
        if (questUIText != null && quest != null)
        {
            questUIText.text = quest.Objective;
        }
    }

    private bool CheckIfQuestCompleted()// Tjekker om den aktive quest er fuldført
    {
        if (playerStats == null)
        {
            Debug.LogWarning("playerStats reference mangler.");
            return false;
        }

        foreach (var requirement in quest.requiredItems)// Gennemgår alle krav i questen
        {
            var playerItem = playerStats.processedItems.Find(x => x.name.Equals(requirement.itemName, System.StringComparison.OrdinalIgnoreCase));// Finder det tilsvarende item i spillerens inventar
            if (playerItem == null || playerItem.amount < requirement.requiredAmount)// Tjekker om itemet ikke findes eller om mængden er mindre end det krævede
            {
                Debug.Log($"Mangler {requirement.requiredAmount}x {requirement.itemName}");
                return false;
            }
        }

        Debug.Log("Alle krav opfyldt!");
        return true;
    }

    private void CompleteQuest()// Fuldfører den aktive quest
    {
        if (playerStats == null)// Tjekker om playerStats referencen er sat
        {
            Debug.LogWarning("playerStats reference mangler.");
            return;
        }

        foreach (var requirement in quest.requiredItems)// Gennemgår alle krav i questen
        {
            var playerItem = playerStats.processedItems.Find(x => x.name.Equals(requirement.itemName, System.StringComparison.OrdinalIgnoreCase));// Finder det tilsvarende item i spillerens inventar
            if (playerItem != null)
            {
                playerItem.amount -= requirement.requiredAmount;// Reducerer mængden af det krævede item i spillerens inventar
            }
        }

        playerStats.playerMoney += quest.Reward;// Tildeler belønningen til spillerens penge
        Debug.Log($"Quest færdig! Du fik {quest.Reward} kr.");

        if (questUIText != null)
        {
            questUIText.text = $"Quest fuldført!\n+{quest.Reward} kr.";// Opdaterer quest UI teksten for at vise belønningen
        }

        quest.isActive = false;// Sætter questen som inaktiv
        quest = null;// Nulstiller quest referencen

        playerStats.UpdateInventoryText();// Opdaterer spillerens inventar UI for at reflektere ændringerne
    }
}
