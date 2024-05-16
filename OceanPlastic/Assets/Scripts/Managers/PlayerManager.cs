using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player player;
    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    private PlayerUpgrades playerUpgrades;
    
    public static PlayerManager Instance { get; private set; }
    private Vector3 startingPlayerPosition;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        player = FindObjectOfType<Player>();
        player.onDeath.AddListener(ResetPlayerOnDeath);
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerUpgrades = FindObjectOfType<PlayerUpgrades>();
        startingPlayerPosition = player.transform.position;
    }
    
    public Player Player
    {
        get { return player; }
    }
    
    public PlayerInventory PlayerInventory
    {
        get { return playerInventory; }
    }
    
    public PlayerMovement PlayerMovement
    {
        get { return playerMovement; }
    }
    
    public PlayerUpgrades PlayerUpgrades
    {
        get { return playerUpgrades; }
    }

    public void ResetPlayerOnDeath()
    {
        Debug.Log("ON PLAYER DEATH");
        playerMovement.transform.position = startingPlayerPosition;
        playerInventory.RemovePercentageOfTrash(0.5f);
        player.Heal(1000000f); // this will be clamped to 100 anyway
    }
}
