using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ThreeCupsController : Minigame
{
    List<Cup> cups = new List<Cup>();
    public int maxSwapCount = 5;
    
    private void Start()
    {
        cups = GetComponentsInChildren<Cup>().ToList();

        foreach (var cup in cups)
        {
            cup.clickable = false;
        }
        MinigameInit();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        foreach (var cup in cups)
        {
            cup.PutDown();
        }

        yield return new WaitForSecondsRealtime(2.3f);

        List<int> indexes = new List<int>{0, 1, 2};
        float x1 = 0;
        float x2 = 0;
        float cupSwapTime = 0.75f;
        for (int i = 0; i < maxSwapCount; i++)
        {
            indexes = indexes.OrderBy(_ => Random.value).ToList();
            x1 = cups[indexes[0]].transform.position.x;
            x2 = cups[indexes[1]].transform.position.x;
            
            cups[indexes[0]].transform.DOMoveX(x2, 0.75f).SetEase(Ease.OutCubic).SetUpdate(true);
            cups[indexes[0]].transform.DOScale(0.75f, cupSwapTime / 4f).OnComplete(
                () =>
                {
                    cups[indexes[0]].transform.DOScale(1f, 3f * cupSwapTime / 4f).SetEase(Ease.OutCubic).SetUpdate(true);
                }).SetEase(Ease.OutCubic).SetUpdate(true);
            
            cups[indexes[1]].transform.DOMoveX(x1, 0.75f).SetEase(Ease.OutCubic).SetUpdate(true);
            cups[indexes[1]].transform.DOScale(0.75f, cupSwapTime / 4f).OnComplete(
                () =>
                {
                    cups[indexes[1]].transform.DOScale(1f, 3f * cupSwapTime / 4f).SetEase(Ease.OutCubic).SetUpdate(true);
                }).SetEase(Ease.OutCubic).SetUpdate(true);
            yield return new WaitForSecondsRealtime(1f);
        }
        foreach (var cup in cups)
        {
            cup.clickable = true;
        }
    }

    public void CheckCup(bool winning)
    {
        if (winning)
        {
            onWin.Invoke();
        }
        else
        {
            onBack.Invoke();
        }
    }
}
