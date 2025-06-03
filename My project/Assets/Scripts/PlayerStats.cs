using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]// Gør klassen serialiserbar, så den kan bruges med JsonUtility
public class ItemEntry// Repræsenterer en post i spillerens inventar
{
    public string name;// Navn på itemet
    public int amount;// Antal af itemet i inventaret
    public int price;// Pris på itemet
    public float weight;//  Vægt af itemet
    public ItemEntry(string name, int amount, int price, float weight)// Konstruktør til at initialisere en ItemEntry
    {
        this.name = name;// Sætter navnet på itemet
        this.amount = amount;// Sætter mængden af itemet
        this.price = price;// Sætter prisen på itemet
        this.weight = weight;// Sætter vægten af itemet
    }
}

public class PlayerStats : MonoBehaviour// Håndterer spillerens stats, herunder inventar, penge og vægt
{
    private DataFetcher dataFetcher;// Referencer til DataFetcher, som henter data om items

    public List<ItemEntry> processedItems = new List<ItemEntry>();// Liste over behandlede items, der indeholder navn, mængde, pris og vægt

    public int playerMoney = 100;// Startbeløb for spilleren
    public float maxCarryWeight = 150f;// Maksimal vægt spilleren kan bære
    public TextMeshProUGUI weightUI;// UI for at vise spillerens nuværende vægt
    public TextMeshProUGUI moneyUI;// UI for at vise spillerens nuværende penge
    public TextMeshProUGUI inventoryUI;// UI for at vise spillerens inventar

    void Start()
    {
        dataFetcher = GetComponent<DataFetcher>();// Henter DataFetcher komponenten fra GameObjectet
        if (dataFetcher == null)
        {
            Debug.LogError("DataFetcher component not found!");
            return;
        }

        StartCoroutine(WaitForData());// Starter coroutine for at vente på, at dataFetcher er klar
    }

    IEnumerator WaitForData()// Coroutine der venter på at dataFetcher er klar
    {
        while (dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)// Tjekker om fetchedItems er klar
        {
            yield return null;
        }

        foreach (ItemData item in dataFetcher.fetchedItems)// Gennemgår alle items i fetchedItems
        {
            processedItems.Add(new ItemEntry(item.Name, 0, item.Price, item.Weight));// Tilføjer hvert item til processedItems listen som en ItemEntry
        }
    }

    public void BuyItem(string itemName)// Metode til at købe et item
    {
        ItemEntry entry = processedItems.Find(x => x.name == itemName);// Finder itemet i processedItems listen baseret på navnet
        if (entry == null)
        {
            Debug.LogWarning($"Item '{itemName}' not found.");
            return;
        }

        float currentWeight = GetTotalWeight();// Henter den nuværende samlede vægt af alle items i inventaret
        float newWeight = currentWeight + entry.weight;// Beregner den nye vægt efter køb af itemet

        if (playerMoney < entry.price)// Tjekker om spilleren har nok penge til at købe itemet
        {
            Debug.Log("Not enough money to buy item.");
            return;
        }

        if (newWeight > maxCarryWeight)// Tjekker om den nye vægt overstiger den maksimale bæregrænse
        {
            Debug.Log($"Cannot carry more. Buying '{itemName}' would exceed your max carry weight of {maxCarryWeight} kg.");
            return;
        }

        playerMoney -= entry.price;// Trækker prisen for itemet fra spillerens penge
        moneyUI.text = $"Money: {playerMoney}";// Opdaterer UI'en for penge
        weightUI.text = $"Weight: {newWeight}/{maxCarryWeight}";// Opdaterer UI'en for vægt
        entry.amount++;// Øger mængden af det købte item i inventaret
        UpdateInventoryText();// Opdaterer inventar UI'en
        Debug.Log($"Bought {itemName} for {entry.price}. New amount: {entry.amount}. Money left: {playerMoney}. Total weight: {newWeight} / {maxCarryWeight}");
    }

    public void SellItem(string itemName)// Metode til at sælge et item
    {
        ItemEntry entry = processedItems.Find(x => x.name == itemName);// Finder itemet i processedItems listen baseret på navnet
        if (entry == null)// Tjekker om itemet findes i listen
        {
            Debug.LogWarning($"Item '{itemName}' not found.");
            return;
        }

        if (entry.amount > 0)// Tjekker om der er nok af itemet til at sælge
        {
            entry.amount--;// Reducerer mængden af itemet i inventaret
            UpdateInventoryText();// Opdaterer inventar UI'en
            playerMoney += entry.price; // Or: entry.price / 2 for half-price
            moneyUI.text = $"Money: {playerMoney}";// Opdaterer UI'en for penge
            float newWeight = GetTotalWeight();// Henter den nye samlede vægt efter salget
            weightUI.text = $"Weight: {newWeight}/{maxCarryWeight}";// Opdaterer UI'en for vægt
            Debug.Log($"Sold {itemName} for {entry.price}. New amount: {entry.amount}. Money now: {playerMoney} Weight: {newWeight}");
        }
        else
        {
            Debug.Log("No more of this item to sell.");
        }
    }

    public float GetTotalWeight()// Beregner den samlede vægt af alle items i inventaret
    {
        float total = 0f;// Initialiserer total vægt til 0
        foreach (var item in processedItems)// Gennemgår alle items i processedItems listen
        {
            total += item.amount * item.weight;// Tilføjer vægten af hvert item ganget med dets mængde til totalen
        }
        return total;// Returnerer den samlede vægt
    }
    public void UpdateInventoryText()// Opdaterer inventar UI'en med de nuværende items og deres mængder
    {
        if (inventoryUI == null) return;// Tjekker om inventoryUI er sat, hvis ikke returnerer metoden

        string result = "Inventory:\n";

        foreach (var item in processedItems)// Gennemgår alle items i processedItems listen
        {
            if (item.amount > 0)// Tjekker om mængden af itemet er større end 0
            {
                result += $"{item.name}: {item.amount}\n";// Tilføjer itemets navn og mængde til resultatstrengen
            }
        }
        inventoryUI.text = result;// Opdaterer inventar UI'en med den samlede tekst
    }
}