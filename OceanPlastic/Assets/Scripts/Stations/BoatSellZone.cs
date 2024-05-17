using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatSellZone : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onOpenSellItemsPanel;
    [SerializeField]
    private UnityEvent onCloseSellItemsPanel;
    
    public UnityEvent<FloatingTrashSO> onSellTrash;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory pi = other.GetComponentInParent<PlayerInventory>();
        if (pi != null)
        {
            onOpenSellItemsPanel.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        onCloseSellItemsPanel.Invoke();
    }

    public void SellItems()
    {
        var playerInventory = PlayerManager.Instance.PlayerInventory;
        List<(FloatingTrashSO, int)> inv = playerInventory.GetInventory();
        for (int i = 0; i < inv.Count; i++)
        {
            var item = inv[i];
            for (int o = 0; o < item.Item2; o++)
            {
                onSellTrash.Invoke(item.Item1);
            }
            playerInventory.AddMoney(item.Item1.value * item.Item2);
        }
        playerInventory.RemoveAllTrash();
    }
}
