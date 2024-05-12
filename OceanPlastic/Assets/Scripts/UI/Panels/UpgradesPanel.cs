using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesPanel : MonoBehaviour
{
    private GameObject root;

    [SerializeField] private GameObject oxygenUpgradesRoot;
    [SerializeField] private GameObject finUpgradeRoot;
    [SerializeField] private GameObject suitUpgradeRoot;
    [SerializeField] private GameObject pocketUpgradeRoot;

    // upgrade entries
    [Header("Upgrade Entries")]
    [SerializeField] private List<UpgradeEntry> oxygenUpgrades;
    [SerializeField] private List<UpgradeEntry> finUpgrades;
    [SerializeField] private List<UpgradeEntry> suitUpgrades;
    [SerializeField] private List<UpgradeEntry> pocketUpgrades;
    
    OxygenUpgrades oxygenUpgrade;
    FinUpgrades finUpgrade; 
    DepthUpgrades suitUpgrade;
    PocketUpgrades pocketUpgrade;
    
    private void Start()
    {
        UpdateUpgrades();
        
        int i = 1;
        oxygenUpgrades.ForEach(entry => { 
            entry.SetUpgradeData(UpgradeType.Oxygen, i);
            entry.onBuy.AddListener(MarkAlreadyBoughtUpgrades);
            i++;
        });
        i = 1;
        finUpgrades.ForEach(entry => { 
            entry.SetUpgradeData(UpgradeType.Fin, i);
            entry.onBuy.AddListener(MarkAlreadyBoughtUpgrades);
            i++;
        });
        i = 1;
        suitUpgrades.ForEach(entry => { 
            entry.SetUpgradeData(UpgradeType.Suit, i);
            entry.onBuy.AddListener(MarkAlreadyBoughtUpgrades);
            i++;
        });
        i = 1;
        pocketUpgrades.ForEach(entry => { 
            entry.SetUpgradeData(UpgradeType.Pocket, i);
            entry.onBuy.AddListener(MarkAlreadyBoughtUpgrades);
            i++;
        });

        
        root = transform.gameObject;
        root.SetActive(false);
        DisableAllTabs();
    }
    
    public void ShowUpgradesPanel()
    {
        root.SetActive(true);
        MarkAlreadyBoughtUpgrades();
    }
    
    public void HideUpgradesPanel()
    {
        root.SetActive(false);
        DisableAllTabs();
    }
    
    public void ShowOxygenUpgrades()
    {
        DisableAllTabs();
        oxygenUpgradesRoot.SetActive(true);
    }
    
    public void ShowFinUpgrades()
    {
        DisableAllTabs();
        finUpgradeRoot.SetActive(true);
    }
    
    public void ShowSuitUpgrades()
    {
        DisableAllTabs();
        suitUpgradeRoot.SetActive(true);
    }
    
    public void ShowPocketUpgrades()
    {
        DisableAllTabs();
        pocketUpgradeRoot.SetActive(true);
    }

    private void DisableAllTabs()
    {
        oxygenUpgradesRoot.SetActive(false);
        finUpgradeRoot.SetActive(false);
        suitUpgradeRoot.SetActive(false);
        pocketUpgradeRoot.SetActive(false);
    }

    private void UpdateUpgrades()
    {
        var upgrades = PlayerManager.Instance.PlayerUpgrades;
        
        oxygenUpgrade = upgrades.GetOxygenUpgrade();
        finUpgrade = upgrades.GetFinUpgrade();
        suitUpgrade = upgrades.GetDepthUpgrade();
        pocketUpgrade = upgrades.GetPocketUpgrade();
    }

    private void MarkAlreadyBoughtUpgrades()
    {
        UpdateUpgrades();
        switch (oxygenUpgrade)
        {
            case OxygenUpgrades.Upgrade2X:
                oxygenUpgrades.Take(1).ToList().ForEach(x => x.SetSold());
                break;
            case OxygenUpgrades.Upgrade4X:
                oxygenUpgrades.Take(2).ToList().ForEach(x => x.SetSold());
                break;
            case OxygenUpgrades.Upgrade8X:
                oxygenUpgrades.Take(3).ToList().ForEach(x => x.SetSold());
                break;
        }
        
        switch (finUpgrade)
        {
            case FinUpgrades.Upgrade133:
                finUpgrades.Take(1).ToList().ForEach(x => x.SetSold());
                break;
            case FinUpgrades.Upgrade166:
                finUpgrades.Take(2).ToList().ForEach(x => x.SetSold());
                break;
            case FinUpgrades.Upgrade200:
                finUpgrades.Take(3).ToList().ForEach(x => x.SetSold());
                break;
        }
        
        switch (suitUpgrade)
        {
            case DepthUpgrades.Upgrade100:
                suitUpgrades.Take(1).ToList().ForEach(x => x.SetSold());
                break;
            case DepthUpgrades.Upgrade200:
                suitUpgrades.Take(2).ToList().ForEach(x => x.SetSold());
                break;
            case DepthUpgrades.Upgrade400:
                suitUpgrades.Take(3).ToList().ForEach(x => x.SetSold());
                break;
        }
        
        switch (pocketUpgrade)
        {
            case PocketUpgrades.Upgrade30:
                pocketUpgrades.Take(1).ToList().ForEach(x => x.SetSold());
                break;
            case PocketUpgrades.Upgrade60:
                pocketUpgrades.Take(2).ToList().ForEach(x => x.SetSold());
                break;
            case PocketUpgrades.Upgrade90:
                pocketUpgrades.Take(3).ToList().ForEach(x => x.SetSold());
                break;
        }
    }
}
