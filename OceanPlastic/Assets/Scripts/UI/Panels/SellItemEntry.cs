using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellItemEntry : MonoBehaviour
{
    [SerializeField]
    Image itemIcon;
    [SerializeField]
    TextMeshProUGUI itemNameText;
    [SerializeField]
    TextMeshProUGUI amountText;
    [SerializeField]
    TextMeshProUGUI valueText;
    [SerializeField]
    TextMeshProUGUI totalText;
    
    public void SetItemEntryValues(Sprite icon, string itemName, int amount, int value, int total)
    {
        itemIcon.sprite = icon;
        itemNameText.text = itemName;
        amountText.text = amount.ToString();
        valueText.text = value.ToString();
        totalText.text = total.ToString();
    }
}
