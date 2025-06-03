using UnityEngine;

public class Upgrades : MonoBehaviour, IInteractable
{
    public GameObject upgradeUI; // UI for upgrades
    public GameObject player; // Reference to the player
    PlayerMovement canmove;
    PlayerStats playerStats;
    PlayerMovement playerMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        upgradeUI.SetActive(true); // Activate the upgrade UI
        PlayerMovement canMove = player.GetComponent<PlayerMovement>();
        canmove.canMove = false; // Disable player movement while in upgrade UI
    }
    void Start()
    {
        canmove = player.GetComponent<PlayerMovement>();
        playerStats = player.GetComponent<PlayerStats>();
        playerMovement = player.GetComponent<PlayerMovement>();
        upgradeUI.SetActive(false); // Ensure the upgrade UI is initially inactive
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            upgradeUI.SetActive(false);
            canmove.canMove = true;
        }
    }
    public void CloseShop()
    {
        upgradeUI.SetActive(false);
        canmove.canMove = true;
    }
    public void UpgradeSpeed()
    {
        if (playerStats.playerMoney >= 100 && playerMovement.moveSpeed <= 9) // Check if the player has enough money
        {
            playerMovement.moveSpeed += 1f; // Increase player speed
            playerStats.playerMoney -= 100; // Deduct the cost from player's money
            Debug.Log("Player speed upgraded!");
            playerStats.moneyUI.text = $"Money: {playerStats.playerMoney}";
        }
        else if (playerMovement.moveSpeed > 9)
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
            playerStats.moneyUI.text = $"Money: {playerStats.playerMoney}";
            playerStats.weightUI.text = $"Weight: {playerStats.GetTotalWeight()}/{playerStats.maxCarryWeight}";
        }
        else if (playerStats.maxCarryWeight > 190)
        {
            Debug.Log("You have reached the maximum weight capacity upgrade limit.");
        }
        else
        {
            Debug.Log("Not enough money to upgrade weight capacity.");
        }
    }
}
