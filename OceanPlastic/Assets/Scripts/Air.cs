using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;
        
        player.SetIsInWater(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;
        
        player.SetIsInWater(true);
    }
}
