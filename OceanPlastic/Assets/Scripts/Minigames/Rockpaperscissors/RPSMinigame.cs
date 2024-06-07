using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RPSMinigame : MonoBehaviour
{
    public TextMeshPro resultText;
    public UnityEvent onWin;
    public UnityEvent onLoseDraw;
    public Transform enemyCard;
    public Transform enemyCardEndPos;
    public List<HandCard> enemyHandCards;
    public Sprite scissorsSprite;
    
    public List<HandCard> handCards;
    
    private void Start()
    {
        foreach (var card in enemyHandCards)
        {
            card.enabled = false;
        }
        
        resultText.transform.localPosition = new Vector3(0, -7f, -1f);
        resultText.text = "";
        resultText.alpha = 0;
    }
    
    public void CardSelected(RPSChoice choice)
    {
        StartCoroutine(StateCheck(choice));
    }

    IEnumerator StateCheck(RPSChoice choice)
    {
        foreach (var card in handCards)
        {
            card.enabled = false;
        }
        enemyHandCards[1].transform.GetComponent<SpriteRenderer>().sprite = scissorsSprite;
        enemyHandCards[1].transform.DOMove(enemyCardEndPos.position, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        enemyHandCards[1].transform.DOScale(enemyCardEndPos.localScale, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        
        yield return new WaitForSecondsRealtime(1f);
        switch (choice)
        {
            case RPSChoice.Rock:
                ShowResult("YOU WIN!", true);
                break;
            case RPSChoice.Paper:
                ShowResult("YOU LOSE", false);
                break;
            case RPSChoice.Scissors:
                ShowResult("DRAW", false);
                break;
        }
    }
    
    private void ShowResult(string result, bool win)
    {
        resultText.text = result;
        resultText.DOFade(1, 1.5f).OnComplete(
            () =>
            {
                resultText.DOFade(0f, 1.5f).SetUpdate(true).OnComplete(() =>
                {
                    if (win)
                    {
                        onWin.Invoke();
                    }
                    else
                    {
                        onLoseDraw.Invoke();
                    }
                });
            }).SetUpdate(true);
        resultText.transform.DOLocalMoveY(0, 1f).SetUpdate(true);
    }
}

public enum RPSChoice
{
    Rock,
    Paper,
    Scissors
}