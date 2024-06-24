using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WhaleMinigame : MinigameTrigger
{
    public List<GameObject> outsideBarnacles;
    
    private void Start()
    {
        MinigameTriggerInit();
        onMinigameHidden.AddListener(AfterMinigameHide);
    }
    
    private void AfterMinigameHide()
    {
        if (!hasPlayerWon) return;
        foreach (var b in outsideBarnacles)
        {
            Destroy(b);
        }
        outsideBarnacles.Clear();
    }
}
