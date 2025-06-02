using System.Collections;
using UnityEngine;

public class Shops : MonoBehaviour, IInteractable
{
    public string navn;
    public GameObject shopUI;
    public DataFetcher dataFetcher;


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

        foreach (ItemData item in dataFetcher.fetchedItems)
        {
            if (name == item.Vendor)
            {
            Debug.Log("Item fra Shops: " + item.Name);
            }

        }
    }
}

