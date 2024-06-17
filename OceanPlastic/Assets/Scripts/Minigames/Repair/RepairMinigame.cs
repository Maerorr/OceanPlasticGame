using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RepairMinigame : Minigame
{
    List<TouchDetectSquare> squares = new List<TouchDetectSquare>();
    public SpriteRenderer background;
    public TextMeshPro repairedText;
    
    private void Start()
    {
        repairedText.text = "";
        foreach (var square in GetComponentsInChildren<TouchDetectSquare>())
        {
            square.onTouched.AddListener(IsAllSquaresTouched);
            squares.Add(square);
        }
        
        MinigameInit();
    }
    
    public void IsAllSquaresTouched()
    {
        foreach (var square in squares)
        {
            if (!square.HasBeenTouched())
            {
                return;
            }
        }

        StartCoroutine(OnWinMessage());
    }
    
    public IEnumerator OnWinMessage()
    {
        /*repairedText.transform.localPosition = new Vector3(0, -7f, -0.5f);
        repairedText.transform.DOMoveY(0f, 1f);*/
        repairedText.rectTransform.anchoredPosition = new Vector2(0f, -7f);
        repairedText.rectTransform.DOAnchorPosY(0f, 1f).SetUpdate(true).SetEase(Ease.OutCubic);
        repairedText.text = "Repaired!";
        yield return new WaitForSecondsRealtime(1.75f);
        onWin.Invoke();
    }
}
