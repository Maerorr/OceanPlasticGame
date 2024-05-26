using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeEntry : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI soldText;
    [SerializeField]
    private TextMeshProUGUI upgradeName;
    [SerializeField]
    private Image upgradeImage;

    [SerializeField] 
    private TextMeshProUGUI costText;

    [SerializeField] private int upgradeCost;

    private int upgradeNumber;
    private UpgradeType upgradeType;
    
    private Button button;

    private Color soldTextColor;
    
    [HideInInspector]
    public UnityEvent onBuy;
    
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        soldTextColor = soldText.color;
        soldText.color = Color.clear;
        costText.text = $"{upgradeCost}g";
    }

    public void SetSold()
    {
        button.interactable = false;
        soldText.color = soldTextColor;
        upgradeImage.color = Color.gray;
        costText.color = Color.clear;
    }

    public void Buy()
    {
        if (!StaticGameData.instance.RemoveMoney(upgradeCost))
        {
            return;
        }
        StaticGameData.instance.AddUpgrade(upgradeType, upgradeNumber);
        onBuy.Invoke();
    }

    public void SetUpgradeData(UpgradeType type, int num)
    {
        upgradeType = type;
        upgradeNumber = num;
    }
}

public enum UpgradeType
{
    Oxygen,
    Fin,
    Suit,
    Pocket
}