using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    OxygenUpgrades oxygenUpgrade = OxygenUpgrades.Basic;
    DepthUpgrades depthUpgrade = DepthUpgrades.Basic;
    BagUpgrades bagUpgrade = BagUpgrades.Basic;

    private Player player;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void ApplyUpgrades()
    {
        player.SetOxygenModifier((float)oxygenUpgrade);
        player.SetMaxSafeDepth((int)depthUpgrade);
        playerInventory.SetBagModifier((int) bagUpgrade);
    }
}

public enum OxygenUpgrades
{
    Basic = 1,
    Upgrade2X = 2,
    Upgrade4X = 4,
    Upgrade8X = 8,
}

public enum DepthUpgrades
{
    Basic = 75,
    Upgrade200 = 200,
    Upgrade400 = 400,
}

public enum BagUpgrades
{
    Basic = 15,
    Upgrade30 = 30,
    Upgrade60 = 60,
    Upgrade90 = 90
}
