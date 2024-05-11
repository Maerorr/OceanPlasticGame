using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player player;
    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    
    public static PlayerManager Instance { get; private set; }
    
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
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMovement = FindObjectOfType<PlayerMovement>();
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
}