using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMinigamePlay : MonoBehaviour
{
    private float minDistance = 10f;
    
    public void StartClosestMinigame(InputAction.CallbackContext ctx)
    {
        if (ctx.phase != InputActionPhase.Performed)
        {
            return;
        }
        var minigames = FindObjectsByType<MinigameTrigger>(FindObjectsSortMode.None);
        var chest = FindAnyObjectByType<Chest>();
        if (minigames.Length == 0)
        {
            return;
        }
        
        MinigameTrigger closestMinigame = null;
        float closestDistance = float.MaxValue;
        foreach (var minigame in minigames)
        {
            float distance = Vector2.Distance(minigame.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMinigame = minigame;
            }
        }
        
        Debug.Log($"Chest distance {Vector2.Distance((Vector2)chest.transform.position, transform.position)}, closest minigame {closestDistance}");
        var chestDistance = Vector2.Distance((Vector2)chest.transform.position, transform.position);
        if ( chestDistance < closestDistance && chestDistance< minDistance)
        {
            chest.OpenChest();
            return;
        }
        
        if (closestDistance < minDistance)
        {
            
            closestMinigame.ShowMinigame();
        }
    }
}
