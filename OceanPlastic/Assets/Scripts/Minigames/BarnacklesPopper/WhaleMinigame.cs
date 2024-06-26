using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WhaleMinigame : MinigameTrigger
{
    public List<GameObject> outsideBarnacles;
    public ParticleSystem heartsAndBubbles;
    
    private void Start()
    {
        MinigameTriggerInit();
        onMinigameHidden.AddListener(AfterMinigameHide);
        heartsAndBubbles.Stop();
    }
    
    private void AfterMinigameHide()
    {
        if (!hasPlayerWon) return;
        foreach (var b in outsideBarnacles)
        {
            Destroy(b);
        }
        outsideBarnacles.Clear();
        StartCoroutine(Bubbles());
    }

    IEnumerator Bubbles()
    {
        heartsAndBubbles.Play();
        yield return new WaitForSecondsRealtime(2f);
        heartsAndBubbles.Stop();
    }
}
