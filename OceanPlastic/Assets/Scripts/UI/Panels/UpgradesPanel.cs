using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesPanel : MonoBehaviour
{
    private GameObject root;

    [SerializeField] private GameObject oxygenUpgradesRoot;
    [SerializeField] private GameObject finUpgradeRoot;
    [SerializeField] private GameObject suitUpgradeRoot;
    [SerializeField] private GameObject pocketUpgradeRoot;

    private void Awake()
    {
        root = transform.gameObject;
        root.SetActive(false);
        DisableAllTabs();
    }
    
    public void ShowUpgradesPanel()
    {
        root.SetActive(true);
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
}
