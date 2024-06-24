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
        initialTooltipLocalY = tooltipTMP.transform.parent.localPosition.y;
        tooltipTMP.text = tooltipText;
        tooltipTMP.color = new Color(1f, 1f, 1f, 0f);
        GetComponentInChildren<NonUIButton>().buttonClicked.AddListener(() =>
        {
            onBack.Invoke();
        });
        var sq = DOTween.Sequence();
        sq.SetUpdate(true);
        sq.Append(tooltipTMP.DOFade(1f, 0.75f).SetEase(Ease.InQuad));
        // here we use .parent because i changed the tooltip to be a child alongside a background. So to move them both we just move the parent.
        sq.Append(tooltipTMP.transform.parent.DOLocalMoveY(initialTooltipLocalY + 4f, 2f).SetDelay(2.5f).SetEase(Ease.OutQuad).OnComplete(
            () => tooltipTMP.transform.parent.gameObject.SetActive(false)));
        sq.Play();
    }
}
