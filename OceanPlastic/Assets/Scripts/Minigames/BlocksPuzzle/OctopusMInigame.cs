using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class OctopusMInigame : MinigameTrigger
{
    public Transform key;
    
    private void Start()
    {
        MinigameTriggerInit();
        key.gameObject.SetActive(false);
        onMinigameHidden.AddListener(AfterMinigameHide);
    }
    
    private void AfterMinigameHide()
    {
        if (!hasPlayerWon) return;
        key.gameObject.SetActive(true);
        key.DOLocalMoveY(5f, 1.5f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(
            () =>
            {
                key.GetComponent<Key>().StartFloating();
            });
        key.DOScale(1f, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}
