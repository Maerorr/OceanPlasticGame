using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player player;
    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    private PlayerUpgrades playerUpgrades;
    private List<TrashSpawner> trashSpawners;
    
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
        
        trashSpawners = new List<TrashSpawner>(FindObjectsOfType<TrashSpawner>());
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
        playerMovement.transform.position = startingPlayerPosition;
        int trashInInv = playerInventory.GetTrashCount();
        int amountToSpawn = Mathf.FloorToInt(trashInInv * 0.66f);
        Debug.Log("Amount To Spawn = " + amountToSpawn);
        playerInventory.RemovePercentageOfTrash(0.5f);
        int currentTrash = 0;
        int maxTrash = 0;
        int spawned = 0;
        // player loses half his inv on death, we need to ensure he can finish the game even after dying
        foreach (var trashSpawner in trashSpawners)
        {
            currentTrash = trashSpawner.GetCurrentObjectsCount();
            maxTrash = trashSpawner.GetMaxTrashCount();
            spawned += trashSpawner.SpawnAdditionalTrash(
                (1 - (float)(maxTrash - currentTrash) / (float)maxTrash) * 0.9f,
                amountToSpawn
            );
            Debug.Log(spawned + " trash spawned");
            if (spawned >= amountToSpawn)
            {
                break;
            }
            amountToSpawn -= spawned;
        }
        player.Heal(1000000f); // this will be clamped to 100 anyway
    }
}
