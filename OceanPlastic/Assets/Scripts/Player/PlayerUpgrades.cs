using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    OxygenUpgrades oxygenUpgrade = OxygenUpgrades.Basic;
    DepthUpgrades depthUpgrade = DepthUpgrades.Basic;
    PocketUpgrades pocketUpgrade = PocketUpgrades.Basic;
    FinUpgrades finUpgrade = FinUpgrades.Basic10;

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
        playerInventory.SetBagModifier((int)pocketUpgrade);
        PrintUpgrades();
    }

    public OxygenUpgrades GetOxygenUpgrade()
    {
        return oxygenUpgrade;
    }

    public DepthUpgrades GetDepthUpgrade()
    {
        return depthUpgrade;
    }

    public PocketUpgrades GetPocketUpgrade()
    {
        return pocketUpgrade;
    }

    public FinUpgrades GetFinUpgrade()
    {
        return finUpgrade;
    }

    public void Upgrade(UpgradeType type, int lvl)
    {
        switch (type)
        {
            case UpgradeType.Oxygen:
                oxygenUpgrade = UpgradeConversions.OxygenFromInt(lvl);
                break;
            case UpgradeType.Fin:
                finUpgrade = UpgradeConversions.FinFromInt(lvl);
                break;
            case UpgradeType.Suit:
                depthUpgrade = UpgradeConversions.DepthFromInt(lvl);
                break;
            case UpgradeType.Pocket:
                pocketUpgrade = UpgradeConversions.PocketFromInt(lvl);
                break;
        }

        ApplyUpgrades();
    }

    private void PrintUpgrades()
    {
        Debug.Log("Oxygen: " + oxygenUpgrade);
        Debug.Log("Depth: " + depthUpgrade);
        Debug.Log("Pocket: " + pocketUpgrade);
        Debug.Log("Fin: " + finUpgrade);
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
        Basic = 50,
        Upgrade100 = 100,
        Upgrade200 = 200,
        Upgrade400 = 400,
    }

    public enum PocketUpgrades
    {
        Basic = 15,
        Upgrade30 = 30,
        Upgrade60 = 60,
        Upgrade90 = 90
    }

    public enum FinUpgrades
    {
        Basic10 = 100,
        Upgrade133 = 133,
        Upgrade166 = 166,
        Upgrade200 = 200
    }

public class UpgradeConversions
{
    public static OxygenUpgrades OxygenFromInt(int i)
    {
        switch (i)
        {
            case 0:
                return OxygenUpgrades.Basic;
            case 1:
                return OxygenUpgrades.Upgrade2X;
            case 2:
                return OxygenUpgrades.Upgrade4X;
            case 3:
                return OxygenUpgrades.Upgrade8X;
        }

        return OxygenUpgrades.Basic;
    }

    public static DepthUpgrades DepthFromInt(int i)
    {
        switch (i)
        {
            case 0:
                return DepthUpgrades.Basic;
            case 1:
                return DepthUpgrades.Upgrade100;
            case 2:
                return DepthUpgrades.Upgrade200;
            case 3:
                return DepthUpgrades.Upgrade400;
        }

        return DepthUpgrades.Basic;
    }

    public static PocketUpgrades PocketFromInt(int i)
    {
        switch (i)
        {
            case 0:
                return PocketUpgrades.Basic;
            case 1:
                return PocketUpgrades.Upgrade30;
            case 2:
                return PocketUpgrades.Upgrade60;
            case 3:
                return PocketUpgrades.Upgrade90;
        }

        return PocketUpgrades.Basic;
    }

    public static FinUpgrades FinFromInt(int i)
    {
        switch (i)
        {
            case 0:
                return FinUpgrades.Basic10;
            case 1:
                return FinUpgrades.Upgrade133;
            case 2:
                return FinUpgrades.Upgrade166;
            case 3:
                return FinUpgrades.Upgrade200;
        }

        return FinUpgrades.Basic10;
    }
}