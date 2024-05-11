using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WarningMessage : MonoBehaviour
{
    private TextMeshPro warningText;
    private void Awake()
    {
        warningText = GetComponent<TextMeshPro>();
    }
    
    public void ShowWarningMessage(string message, Vector3 startPosition, Color color, float time)
    {
        warningText.text = message;
        warningText.color = color;
        transform.position = startPosition;
        var currentY = transform.position.y;
        var targetY = currentY + 1;
        // tween the position and color to alpha 0
        transform.DOMoveY(targetY, time).SetEase(Ease.OutCirc);
        warningText.DOFade(0, time).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
