using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class QuestGiver : MonoBehaviour, IInteractable
{
    public Quest quest;
    public DataFetcher dataFetcher;
    public TMP_Text questUIText;
    public PlayerStats playerStats;

    void Start()
    {
        quest = null;
        // Automatisk find PlayerStats hvis ikke sat
        if (playerStats == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerStats = playerObj.GetComponent<PlayerStats>();
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with QuestGiver");
        if (quest == null)
        {
            GenerateQuest();
        }
        else if (quest.isActive && CheckIfQuestCompleted())
        {
            CompleteQuest();
        }
        else
        {
            Debug.Log("Quest aktiv men endnu ikke færdiggjort.");
            ShowQuestInUI(); // Vis den aktive quest igen
        }
    }

    private void GenerateQuest()
    {
        Debug.Log("Forsøger at generere ny quest...");

        if (dataFetcher == null || dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)
        {
            Debug.LogWarning("DataFetcher eller fetchedItems er ikke klar.");
            return;
        }

        quest = new Quest();
        quest.isActive = true;
        quest.Reward = Random.Range(150, 250);

        var shuffledItems = dataFetcher.fetchedItems.OrderBy(x => Random.value).ToList();
        int itemCount = Random.Range(1, 4);

        for (int i = 0; i < itemCount; i++)
        {
            var item = shuffledItems[i];
            int requiredAmount = Random.Range(1, 4);

            quest.requiredItems.Add(new QuestItemRequirement
            {
                itemId = item.ItemID,
                itemName = item.Name,
                requiredAmount = requiredAmount
            });
        }

        string objectiveText = "Saml: ";
        foreach (var req in quest.requiredItems)
        {
            objectiveText += $"{req.requiredAmount}x {req.itemName}, ";
        }

        quest.Objective = objectiveText.TrimEnd(',', ' ');

        Debug.Log("Quest genereret: " + quest.Objective);
        ShowQuestInUI();
    }

    private void ShowQuestInUI()
    {
        if (questUIText != null && quest != null)
        {
            questUIText.text = quest.Objective;
        }
    }

    private bool CheckIfQuestCompleted()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("playerStats reference mangler.");
            return false;
        }

        foreach (var requirement in quest.requiredItems)
        {
            var playerItem = playerStats.processedItems.Find(x => x.name.Equals(requirement.itemName, System.StringComparison.OrdinalIgnoreCase));
            if (playerItem == null || playerItem.amount < requirement.requiredAmount)
            {
                Debug.Log($"Mangler {requirement.requiredAmount}x {requirement.itemName}");
                return false;
            }
        }

        Debug.Log("Alle krav opfyldt!");
        return true;
    }

    private void CompleteQuest()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("playerStats reference mangler.");
            return;
        }

        foreach (var requirement in quest.requiredItems)
        {
            var playerItem = playerStats.processedItems.Find(x => x.name.Equals(requirement.itemName, System.StringComparison.OrdinalIgnoreCase));
            if (playerItem != null)
            {
                playerItem.amount -= requirement.requiredAmount;
            }
        }

        playerStats.playerMoney += quest.Reward;
        Debug.Log($"Quest færdig! Du fik {quest.Reward} kr.");

        if (questUIText != null)
        {
            questUIText.text = $"Quest fuldført!\n+{quest.Reward} kr.";
        }

        quest.isActive = false;
        quest = null;

        playerStats.UpdateInventoryText();
    }
}
