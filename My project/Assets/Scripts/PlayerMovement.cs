using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         // Movement speed of the player
    private Rigidbody rb;// Reference to the Rigidbody component for physics-based movement
    private Vector3 movement;// Stores the movement direction based on player input
    public bool canMove = true;// Flag to control whether the player can move
    void Start()
    {
        rb = GetComponent<Rigidbody>();// Get the Rigidbody component attached to the player GameObject
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right arrows)
        float moveZ = Input.GetAxisRaw("Vertical");   // Get vertical input (W/S or Up/Down arrows)

        movement = new Vector3(moveX, 0f, moveZ).normalized;// Create a movement vector based on input, ensuring it is normalized to prevent faster diagonal movement
    }

    void FixedUpdate()
    {
        if (canMove)// Check if the player is allowed to move
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);// Move the player by applying the movement vector scaled by moveSpeed and Time.fixedDeltaTime for smooth movement
        }

    }
}