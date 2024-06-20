using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent onWin;
    public UnityEvent onBack;

    public string tooltipText;
    public TextMeshPro tooltipTMP;
    private float initialTooltipLocalY;

    protected void MinigameInit()
    {
        tooltipTMP = GameObject.Find("Tooltip").GetComponent<TextMeshPro>();
        initialTooltipLocalY = tooltipTMP.transform.localPosition.y;
        tooltipTMP.text = tooltipText;
        tooltipTMP.color = new Color(1f, 1f, 1f, 0f);
        GetComponentInChildren<NonUIButton>().buttonClicked.AddListener(() =>
        {
            onBack.Invoke();
        });
        var sq = DOTween.Sequence();
        sq.SetUpdate(true);
        sq.Append(tooltipTMP.DOFade(1f, 1.5f).SetEase(Ease.InQuad));
        sq.Append(tooltipTMP.transform.DOLocalMoveY(initialTooltipLocalY + 6f, 2f).SetDelay(4f).SetEase(Ease.OutQuad).OnComplete(
            () => tooltipTMP.gameObject.SetActive(false)));
        sq.Play();
    }
}
