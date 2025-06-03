using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class QuestGiver : MonoBehaviour, IInteractable
{
    public Quest quest;
    public DataFetcher dataFetcher;
    public TMP_Text questUIText;
    public void Interact()
    {
        if (quest == null)
        {
            GenerateQuest();
        }
        else if (quest.isActive && CheckIfQuestCompleted())
        {
            CompleteQuest();
        }
    }

    private void GenerateQuest()
    {
        if (dataFetcher == null || dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)
        {
            Debug.LogWarning("DataFetcher or item list is not ready.");
            return;
        }

        quest = new Quest();
        quest.isActive = true;
        quest.Reward = Random.Range(20, 101); // penge har ikke bestemt mængde endnu 

        // Vælg 1–3 tilfældige unikke varer fra databasen
        var shuffledItems = dataFetcher.fetchedItems.OrderBy(x => Random.value).ToList();
        int itemCount = Random.Range(1, 4);

        for (int i = 0; i < itemCount; i++)
        {
            var item = shuffledItems[i];
            int requiredAmount = Random.Range(1, 4); // en mængde af de varerr 1-3

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

        Debug.Log("Ny quest genereret: " + quest.Objective);
        ShowQuestInUI();
    }
    void ShowQuestInUI()
    {
        if (questUIText != null && quest != null)
        {
            questUIText.text = quest.Objective;
        }
    }
    private bool CheckIfQuestCompleted()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogWarning("PlayerStats not found.");
            return false;
        }

        foreach (var requirement in quest.requiredItems)
        {
            var playerItem = player.processedItems.Find(x => x.name == requirement.itemName);
            if (playerItem == null || playerItem.amount < requirement.requiredAmount)
            {
                return false; // Mangler item eller for lidt
            }
        }

        return true; // Har alt
    }
    private void CompleteQuest()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player == null)
        {
            Debug.LogWarning("PlayerStats not found.");
            return;
        }

        // Fjerne items fra spillerens inventar
        foreach (var requirement in quest.requiredItems)
        {
            var playerItem = player.processedItems.Find(x => x.name == requirement.itemName);
            if (playerItem != null)
            {
                playerItem.amount -= requirement.requiredAmount;
            }
        }

        // Give belønning
        player.playerMoney += quest.Reward;
        Debug.Log($"Quest færdig! Du fik {quest.Reward} penge.");

        // Opdatere UI
        if (questUIText != null)
        {
            questUIText.text = $"Afleveret! Du fik {quest.Reward} kr.";
        }

        quest.isActive = false;
        quest = null;

        // Opdatere spillerens inventory UI (hvis du har en metode til det)
        player.UpdateInventoryText();
    }
}