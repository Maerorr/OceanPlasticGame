using UnityEngine;
using UnityEngine.Events;

public class StaticGameData : MonoBehaviour
{
    public static StaticGameData instance;

    #region UPGRADES

    [HideInInspector] public OxygenUpgrades oxygenUpgrade = OxygenUpgrades.Basic;
    [HideInInspector] public DepthUpgrades depthUpgrade = DepthUpgrades.Basic;
    [HideInInspector] public PocketUpgrades pocketUpgrade = PocketUpgrades.Basic;
    [HideInInspector] public FinUpgrades finUpgrade = FinUpgrades.Basic10;
    [HideInInspector] public int money;
    [HideInInspector] public UnityEvent onMoneyChange;
    #endregion
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public bool RemoveMoney(int amount)
    {
        if (money - amount < 0)
        {
            return false;
        }

        money -= amount;
        onMoneyChange.Invoke();
        return true;
    }

    public void AddUpgrade(UpgradeType type, int lvl)
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
    }
}
