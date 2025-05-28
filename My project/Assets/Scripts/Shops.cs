using UnityEngine;

public class Shops : MonoBehaviour, IInteractable
{
    string navn;
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

