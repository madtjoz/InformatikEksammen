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
    public GameObject[] UIList;
    public List<int> IDList = new List<int>();

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
        while (dataFetcher == null)
        {
            yield return null;
        }

        while (dataFetcher.fetchedItems == null || dataFetcher.fetchedItems.Count == 0)
        {
            yield return null;
        }

        foreach (ItemData item in dataFetcher.fetchedItems.Where(item => item.Vendor == navn))
        {
            IDList.Add(item.ItemID);
            Debug.Log(IDList.Count);
        }

        for (int i = 0; i < UIList.Length / 3; i++)
        {
            Debug.Log("ting2");
            int søgtId = IDList[i];
            ItemData item2 = dataFetcher.fetchedItems.FirstOrDefault(i => i.ItemID == søgtId);
            if (item2 != null)
            {
                UIList[i*3].GetComponent<TextMeshProUGUI>().text = $"{item2.Name}";
                UIList[i * 3 + 1].GetComponent<TextMeshProUGUI>().text = $"Price: {item2.Price}";
                UIList[i * 3 + 2].GetComponent<TextMeshProUGUI>().text = $"Weight: {item2.Weight}";
                Debug.Log("ting");
            }
        }
    }
}

