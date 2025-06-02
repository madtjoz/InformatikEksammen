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
        // Vent til data er hentet
        while (dataFetcher.fetchedItems == null)
        {
            yield return null;
        }

        // Brug data
        foreach (ItemData item in dataFetcher.fetchedItems)
        {
            if (navn == item.Vendor)
            {
                Debug.Log("Item from ItemManager: " + item.Name);
            }
        }
    }
}

