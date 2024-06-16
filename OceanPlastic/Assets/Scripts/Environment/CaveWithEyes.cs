using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CaveWithEyes : MinigameTrigger
{
    [SerializeField]
    private Sprite noEyesSprite;
    [SerializeField]
    private Sprite eyesSprite;
    SpriteRenderer spriteRenderer;
    public Transform key;
    
    private void Start()
    {
        MinigameTriggerInit();
        spriteRenderer = GetComponent<SpriteRenderer>();
        key.gameObject.SetActive(false);
        StartCoroutine(Blink());
        onMinigameHidden.AddListener(AfterMinigameHide);
    }
    
    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            spriteRenderer.sprite = noEyesSprite;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = eyesSprite; 
        }
    }

    private void AfterMinigameHide()
    {
        if (!hasPlayerWon) return;
        key.gameObject.SetActive(true);
        key.DOLocalMoveY(3f, 1.5f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(
            () =>
            {
                key.GetComponent<Key>().StartFloating();
            });
        key.DOScale(1f, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}
