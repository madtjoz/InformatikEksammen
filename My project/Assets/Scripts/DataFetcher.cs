using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable] // Gør klassen serialiserbar, så den kan bruges med JsonUtility
public class ItemData // Repræsenterer data for et item
{
    public int ItemID; // Unik ID for itemet
    public string Name;// Navn på itemet
    public string Vendor;// Navn på sælgeren af itemet
    public int Price;// Pris på itemet
    public int Weight;// Vægt af itemet
}

[System.Serializable]// Gør klassen serialiserbar, så den kan bruges med JsonUtility
public class ItemDataList// Repræsenterer en liste af ItemData
{
    public List<ItemData> Items;// Liste over ItemData objekter
}

public class DataFetcher : MonoBehaviour// Henter data fra en ekstern kilde (f.eks. en webserver) og gemmer det i en liste
{
    public List<ItemData> fetchedItems; // <-- Denne liste er tilgængelig fra andre scripts

    void Start()
    {
        StartCoroutine(GetData());// Starter coroutine for at hente data
    }

    IEnumerator GetData()// Coroutine der henter data fra en ekstern kilde
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/eksammen/fetch_scores.php");// URL til den eksterne kilde, hvor data hentes fra

        yield return www.SendWebRequest();// Sender forespørgslen og venter på svar

        if (www.result != UnityWebRequest.Result.Success)// Tjekker om forespørgslen var succesfuld
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("RAW DATA: " + www.downloadHandler.text);

            string json = "{\"Items\":" + www.downloadHandler.text + "}"; // Tilføjer en wrapper for at matche ItemDataList strukturen

            ItemDataList itemDataList = JsonUtility.FromJson<ItemDataList>(json);// Deserialiserer den modtagne JSON-data til en ItemDataList objekt
            ItemDataList itemData = itemDataList;

            fetchedItems = itemData.Items; // Gemmer de hentede items i fetchedItems listen
        }
    }
}