using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemData
{
    public int ItemId;
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
            Debug.Log(www.downloadHandler.text);
            string json = "{\"Items\":" + www.downloadHandler.text + "}";

            ItemDataList itemData = JsonUtility.FromJson<ItemDataList>(json);

            foreach (ItemData item in itemData.Items)
            {
                //Debug.Log($"Item ID: {item.ItemId}, Navn: {item.Name}, Vednor: {item.Vendor}, Price: {item.Price}, weight: {item.Weight}");
                break;
            }
            Debug.Log($"Item ID: {itemData.Items[3].Name}");

        }
    }
}
