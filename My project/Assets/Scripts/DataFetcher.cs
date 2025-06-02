using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ItemData
{
    public int ItemID;
    public string Name;
    public string Vendor;
    public int Price;
    public int Weight;
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemData> Items;
}

public class DataFetcher : MonoBehaviour
{
    public List<ItemData> fetchedItems; // <-- Denne liste er tilgængelig fra andre scripts

    void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/eksammen/fetch_scores.php");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string json = "{\"Items\":" + www.downloadHandler.text + "}";

            ItemDataList itemData = JsonUtility.FromJson<ItemDataList>(json);

            fetchedItems = itemData.Items;

            foreach (ItemData item in fetchedItems)
            {
                //Debug.Log($"Item ID: {item.ItemId}, Navn: {item.Name}, Vednor: {item.Vendor}, Price: {item.Price}, weight: {item.Weight}");
            }
        }
    }
}