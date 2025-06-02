using UnityEngine;
using UnityEngine.UI;

public class Shops : MonoBehaviour, IInteractable
{
    public string navn;
    public int[] itemID;
    public string[] items;
    public float[] prices;
    public float[] weight;



    void Start()
    {
        navn = gameObject.name;
    }

    public void Interact()
    {
        
        if (navn == "Seller 1 - Gronthandler")
        {

            Debug.Log(navn);
        }
        else if (navn == "Slagter")
        {
            Debug.Log(navn);
        }
        else if (navn == "Seller 3 - Skov_hugger")
        {
            Debug.Log(navn);
        }
        else if (navn == "Seller 4 - Miner")
        {
            Debug.Log(navn);
        }
        else
        {
            Debug.Log("Intet");
        }
    }
}

