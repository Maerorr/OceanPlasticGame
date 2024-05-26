using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI moneyText;

    private void Start()
    {
        UpdateMoney();
        StaticGameData.instance.onMoneyChange.AddListener(UpdateMoney);
    }

    public void UpdateMoney()
    {
        int money = StaticGameData.instance.money;
        moneyText.text = $"{money}g";
    }
}
