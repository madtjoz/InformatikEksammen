using UnityEngine;

public class Upgrades : MonoBehaviour, IInteractable // This class implements the IInteractable interface to allow interaction with the upgrade station
{
    public GameObject upgradeUI; // UI for upgrades
    public GameObject player; // Reference to the player
    PlayerMovement canmove; // Reference to PlayerMovement to control player movement
    PlayerStats playerStats; // Reference to PlayerStats to manage player stats
    PlayerMovement playerMovement; // Reference to PlayerMovement to manage player movement speed



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact() // This method is called when the player interacts with the upgrade station   
    {
        upgradeUI.SetActive(true); // Activate the upgrade UI
        PlayerMovement canMove = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement component from the player
        canmove.canMove = false; // Disable player movement while in upgrade UI
    }
    void Start() // This method is called when the script instance is being loaded
    {
        canmove = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement component from the player
        playerStats = player.GetComponent<PlayerStats>(); // Get the PlayerStats component from the player
        playerMovement = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement component from the player
        upgradeUI.SetActive(false); // Ensure the upgrade UI is initially inactive
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))// Check if the Escape key is pressed
        {
            upgradeUI.SetActive(false); // Deactivate the upgrade UI
            canmove.canMove = true; // Re-enable player movement
        }
    }
    public void CloseShop() // This method is called to close the upgrade UI
    {
        upgradeUI.SetActive(false); // Deactivate the upgrade UI
        canmove.canMove = true;// Re-enable player movement
    }
    public void UpgradeSpeed()// This method is called to upgrade the player's speed
    {
        if (playerStats.playerMoney >= 100 && playerMovement.moveSpeed <= 9) // Check if the player has enough money
        {
            playerMovement.moveSpeed += 1f; // Increase player speed
            playerStats.playerMoney -= 100; // Deduct the cost from player's money
            Debug.Log("Player speed upgraded!");
            playerStats.moneyUI.text = $"Money: {playerStats.playerMoney}";// Update the money UI
        }
        else if (playerMovement.moveSpeed > 9)// Check if the player has reached the maximum speed upgrade limit
        {
            Debug.Log("You have reached the maximum speed upgrade limit.");
        }
        else
        {
            Debug.Log("Not enough money to upgrade speed.");
        }
    }
    public void UpgradeWeight()
    {
        if (playerStats.playerMoney >= 100 && playerStats.maxCarryWeight <= 190) // Check if the player has enough money
        {
            playerStats.maxCarryWeight += 10; // Increase player's max carry weight
            playerStats.playerMoney -= 100; // Deduct the cost from player's money
            Debug.Log("Player weight capacity upgraded!");
            playerStats.moneyUI.text = $"Money: {playerStats.playerMoney}";// Update the money UI
            playerStats.weightUI.text = $"Weight: {playerStats.GetTotalWeight()}/{playerStats.maxCarryWeight}";// Update the weight UI
        }
        else if (playerStats.maxCarryWeight > 190)// Check if the player has reached the maximum weight capacity upgrade limit
        {
            Debug.Log("You have reached the maximum weight capacity upgrade limit.");
        }
        else
        {
            Debug.Log("Not enough money to upgrade weight capacity.");
        }
    }
}
