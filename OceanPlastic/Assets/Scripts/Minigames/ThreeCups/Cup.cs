using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Cup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isCorrect = false;
    public Transform sprite;
    public bool clickable = false;
    ThreeCupsController threeCupsController;
    private float initialY;

    private void Start()
    {
        threeCupsController = GetComponentInParent<ThreeCupsController>();
        initialY = sprite.localPosition.y;
    }

    public void PutDown()
    {
        sprite.DOLocalMoveY(0f, Random.Range(1.6f, 2.1f)).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void PutUp()
    {
        sprite.DOLocalMoveY(initialY, 1.5f).SetEase(Ease.InOutCubic).SetUpdate(true);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!clickable) return;
        PutUp();
        threeCupsController.CheckCup(isCorrect);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!clickable) return;
    }
}
