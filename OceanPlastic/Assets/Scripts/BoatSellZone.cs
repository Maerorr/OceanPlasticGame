using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSellZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory pi = other.GetComponentInParent<PlayerInventory>();
        if (pi != null)
        {
            SellItems(pi);
        }
    }

    private void SellItems(PlayerInventory pi)
    {
        List<FloatingTrashSO> inv = pi.GetInventory();
        for (int i = 0; i < inv.Count; i++)
        {
            var item = inv[i];
            pi.AddMoney(item.value);
        }
        pi.RemoveAllTrash();
    }
}
