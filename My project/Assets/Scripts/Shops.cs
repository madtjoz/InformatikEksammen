using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
public class Shops : MonoBehaviour, IInteractable
{
    public string navn;
    public GameObject shopUI;
    public DataFetcher dataFetcher;
    public TextMeshProUGUI itemWeightUI;
    public TextMeshProUGUI itemPriceUI;
    public TextMeshProUGUI itemNameUI;
    public GameObject[] UIlist;
    List<int> IDList = new List<int>();

    private void Start()

    {
        navn = gameObject.name;
        shopUI.SetActive(false);
        StartCoroutine(UseDataWhenReady());
    }

    public void Interact()
    {
        shopUI.SetActive(true);
        Debug.Log("");
    }

    void Update()
    {
        //Gør så Shop kan lukkes når escape rammes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopUI.SetActive(false);
        }
    }
    IEnumerator UseDataWhenReady()
    {
        Debug.Log("Shops: Venter på dataFetcher...");
        while (dataFetcher == null)
        {
            yield return null;
        }

        Debug.Log("Shops: dataFetcher fundet. Venter på data...");
        while (dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)
        {
            yield return null;
        }

        Debug.Log("Shops: Data modtaget!");


        foreach (ItemData item in dataFetcher.fetchedItems.Where(item => item.Vendor == navn))
        {
            IDList.Add(item.ItemId);
            Debug.Log(IDList.Count);
            //itemNameUI.text = item.Name;
            //itemWeightUI.text = $"Weights - {item.Weight}";
            //itemPriceUI.text = $"Price - {item.Price}";
        }
        for (int i = 0; i < UIlist.Length; i++)
        {
            UIlist.
        }
    }
}

