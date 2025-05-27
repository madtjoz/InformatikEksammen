using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactionDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r= new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
