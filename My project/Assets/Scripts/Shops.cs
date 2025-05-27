using UnityEngine;

public class Shops : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log(Random.Range(0, 1000));
    }
}
