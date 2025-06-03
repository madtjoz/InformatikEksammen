using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
public class Shops : MonoBehaviour, IInteractable // IInteractable interface for interaction functionality
{
    public string navn;// Navn på butikken, bruges til at finde items i DataFetcher
    public GameObject shopUI;// UI for butikken, aktiveres ved interaktion
    public GameObject player;// Referencer til spilleren, bruges til at deaktivere bevægelse under interaktion
    public DataFetcher dataFetcher;// Referencer til DataFetcher, som henter data om items
    public GameObject[] UIList;// Referencer til UI-elementer, hvor items vises i butikken
    public List<int> IDList = new List<int>();// Liste over item IDs, der hentes fra DataFetcher
    PlayerMovement canmove;// Referencer til PlayerMovement, bruges til at deaktivere bevægelse under interaktion


    private void Start()// Start-metode, initialiserer variabler og starter coroutine for at hente data

    {
        navn = gameObject.name;// Sætter butikkens navn til GameObject's navn
        shopUI.SetActive(false);//  Deaktiverer shopUI ved start
        canmove = player.GetComponent<PlayerMovement>();// Henter PlayerMovement-komponenten fra spilleren
        StartCoroutine(UseDataWhenReady());// Starter coroutine for at vente på, at data er klar
    }

    public void Interact()// Interact-metode, der kaldes når spilleren interagerer med butikken
    {
        shopUI.SetActive(true); // Aktiverer shopUI for at vise butikkens interface
        Debug.Log("");
        canmove.canMove = false;    // Deaktiverer spillerens bevægelse under interaktion
    }

    void Update()// Update-metode, der kører hver frame
    {
        if (Input.GetKeyDown(KeyCode.Escape))// Tjekker om Escape-tasten trykkes
        {
            shopUI.SetActive(false);// Deaktiverer shopUI for at lukke butikken
            canmove.canMove = true;// Aktiverer spillerens bevægelse igen
        }
    }
    IEnumerator UseDataWhenReady()// Coroutine, der venter på at data er klar før den fortsætter
    {
        while (dataFetcher == null)
        {
            yield return null;
        }

        while (dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0) // Tjekker om fetchedItems er klar
        {
            yield return null; // Venter indtil data er hentet
        }

        foreach (ItemData item in dataFetcher.fetchedItems.Where(item => item.Vendor == navn)) // Filtrerer items baseret på butikkens navn
        {
            IDList.Add(item.ItemID); // Tilføjer item ID'er til IDList
        }

        for (int i = 0; i < UIList.Length / 3; i++) // Gennemgår UIList i trin af 3, da hvert item vises i 3 UI-elementer
        {
            int søgtId = IDList[i]; // Henter item ID fra IDList baseret på indekset
            ItemData item2 = dataFetcher.fetchedItems.FirstOrDefault(i => i.ItemID == søgtId); // Finder det første item i fetchedItems, der matcher det søgte ID
            if (item2 != null)// Tjekker om item2 ikke er null
            {
                UIList[i * 3].GetComponent<TextMeshProUGUI>().text = $"{item2.Name}"; // Sætter navnet på itemet i det første UI-element
                UIList[i * 3 + 1].GetComponent<TextMeshProUGUI>().text = $"Price: {item2.Price}"; //Sætter prisen på itemet i det andet UI-element
                UIList[i * 3 + 2].GetComponent<TextMeshProUGUI>().text = $"Weight: {item2.Weight}"; //Sætter vægten på itemet i det tredje UI-element
            }
        }
    }
    public void CloseShop()// Metode til at lukke butikken
    {
        shopUI.SetActive(false);// Deaktiverer shopUI for at lukke butikken
        canmove.canMove = true;// Aktiverer spillerens bevægelse igen
    }
}

