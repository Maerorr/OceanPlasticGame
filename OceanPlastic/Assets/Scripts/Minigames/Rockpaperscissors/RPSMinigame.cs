using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RPSMinigame : Minigame
{
    public TextMeshPro resultText;
    public Transform enemyCard;
    public Transform enemyCardEndPos;
    public List<HandCard> enemyHandCards;
    public Sprite scissorsSprite;
    public Sprite bubbleSprite;
    
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
        
        MinigameInit();
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
        enemyHandCards[1].transform.GetComponent<SpriteRenderer>().sprite = bubbleSprite;
        GameObject scissors = new GameObject("enemy scissors", typeof(SpriteRenderer));
        scissors.GetComponent<SpriteRenderer>().sprite = scissorsSprite;
        scissors.transform.parent = enemyHandCards[1].transform;
        scissors.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        scissors.transform.rotation = Quaternion.Euler(0, 0, -75);
        scissors.transform.localPosition = new Vector3(0f, 0f, 0.05f);
        enemyHandCards[1].transform.DOMove(enemyCardEndPos.position, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        enemyHandCards[1].transform.DOScale(enemyCardEndPos.localScale, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        enemyHandCards[1].transform.DORotate(enemyCardEndPos.rotation.eulerAngles, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        
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
                        onBack.Invoke();
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