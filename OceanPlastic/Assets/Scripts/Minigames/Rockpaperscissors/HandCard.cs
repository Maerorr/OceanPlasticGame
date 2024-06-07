using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private RPSMinigame minigame;
    [SerializeField]
    RPSChoice choice;

    public bool enabled = true;

    public Transform endCardPos;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enabled) return;
        minigame.CardSelected(choice);
        transform.DOMove(endCardPos.position, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
    }
}
