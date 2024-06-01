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
        if (itemIcon != null)
        {
            itemIcon.sprite = icon;
        }
        if (itemNameText != null)
        {
            itemNameText.text = itemName;
        }
        if (amountText != null)
        {
            amountText.text = amount.ToString();
        }
        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
        if (totalText != null)
        {
            totalText.text = total.ToString();
        }
    }
}
