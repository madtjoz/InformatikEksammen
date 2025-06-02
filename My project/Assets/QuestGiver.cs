using Unity.VisualScripting;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public void Interact()
    {
        Debug.Log(Random.Range(0, 1000));
    }

}
