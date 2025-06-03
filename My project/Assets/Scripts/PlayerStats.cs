using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ItemEntry
{
    public string name;
    public int amount;
    public int price;
    public float weight;

    public ItemEntry(string name, int amount, int price, float weight)
    {
        this.name = name;
        this.amount = amount;
        this.price = price;
        this.weight = weight;
    }
}

public class PlayerStats : MonoBehaviour
{
    private DataFetcher dataFetcher;

    public List<ItemEntry> processedItems = new List<ItemEntry>();

    public int playerMoney = 100;
    public float maxCarryWeight = 150f;
    public TextMeshProUGUI weightUI;
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI inventoryUI;

    void Start()
    {
        dataFetcher = GetComponent<DataFetcher>();
        if (dataFetcher == null)
        {
            Debug.LogError("DataFetcher component not found!");
            return;
        }

        StartCoroutine(WaitForData());
    }

    IEnumerator WaitForData()
    {
        while (dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)
        {
            yield return null;
        }

        foreach (ItemData item in dataFetcher.fetchedItems)
        {
            processedItems.Add(new ItemEntry(item.Name, 0, item.Price, item.Weight));
        }
    }

    public void BuyItem(string itemName)
    {
        ItemEntry entry = processedItems.Find(x => x.name == itemName);
        if (entry == null)
        {
            Debug.LogWarning($"Item '{itemName}' not found.");
            return;
        }

        float currentWeight = GetTotalWeight();
        float newWeight = currentWeight + entry.weight;

        if (playerMoney < entry.price)
        {
            Debug.Log("Not enough money to buy item.");
            return;
        }

        if (newWeight > maxCarryWeight)
        {
            Debug.Log($"Cannot carry more. Buying '{itemName}' would exceed your max carry weight of {maxCarryWeight} kg.");
            return;
        }

        playerMoney -= entry.price;
        moneyUI.text = $"Money: {playerMoney}";
        weightUI.text = $"Weight: {newWeight}/{maxCarryWeight}";
        entry.amount++;
        UpdateInventoryText();
        Debug.Log($"Bought {itemName} for {entry.price}. New amount: {entry.amount}. Money left: {playerMoney}. Total weight: {newWeight} / {maxCarryWeight}");
    }

    public void SellItem(string itemName)
    {
        ItemEntry entry = processedItems.Find(x => x.name == itemName);
        if (entry == null)
        {
            Debug.LogWarning($"Item '{itemName}' not found.");
            return;
        }

        if (entry.amount > 0)
        {
            entry.amount--;
            UpdateInventoryText();
            playerMoney += entry.price; // Or: entry.price / 2 for half-price
            moneyUI.text = $"Money: {playerMoney}";
            float newWeight = GetTotalWeight();
            weightUI.text = $"Weight: {newWeight}/{maxCarryWeight}";
            Debug.Log($"Sold {itemName} for {entry.price}. New amount: {entry.amount}. Money now: {playerMoney} Weight: {newWeight}");
        }
        else
        {
            Debug.Log("No more of this item to sell.");
        }
    }

    public float GetTotalWeight()
    {
        float total = 0f;
        foreach (var item in processedItems)
        {
            total += item.amount * item.weight;
        }
        return total;
    }
    public void UpdateInventoryText()
    {
        if (inventoryUI == null) return;

        string result = "Inventory:\n";

        foreach (var item in processedItems)
        {
            if (item.amount > 0)
            {
                result += $"{item.name}: {item.amount}\n";
            }
        }
        inventoryUI.text = result;
    }
}