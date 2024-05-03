using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI moneyText;

    public void SetMoneyValue(int money)
    {
        moneyText.text = $"{money}g";
    }
}
