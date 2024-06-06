using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CaveWithEyes : MonoBehaviour
{
    [SerializeField]
    private Sprite noEyesSprite;
    [SerializeField]
    private Sprite eyesSprite;
    SpriteRenderer spriteRenderer;
    
    private GameUIController gameUIController;
    private ToolButtons toolButtons;
    
    private bool isPlayerInside = false;
    private bool hasPlayerWon = false;
    public UnityEvent onWin;
    
    public GameObject minigamePrefab;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }
    
    private IEnumerator Blink()
    {
        while (true)
        {
            if (Vector3.Distance(PlayerManager.Instance.Position(), transform.position) < 7f)
            {
                spriteRenderer.sprite = noEyesSprite;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                spriteRenderer.sprite = noEyesSprite;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = eyesSprite; 
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayerWon) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerInside = true;
                toolButtons.EnableExtraButton();
                toolButtons.SetTooltip("Tap to play!");
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (hasPlayerWon) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerInside = false;
                toolButtons.DisableExtraButton();
                toolButtons.ClearTooltip();
            }
        }
    }

    private void MinigameWon()
    {
        
    }

    private void MinigameLost()
    {
        
    }

    private void StartMinigame()
    {
        if (!isPlayerInside) return;
        if (hasPlayerWon) return;
        toolButtons.DisableExtraButton();
        toolButtons.ClearTooltip();
        var minigame = Instantiate(minigamePrefab, Vector3.zero, Quaternion.identity);
        minigame.transform.parent = Camera.main.transform;
        minigame.transform.localPosition = new Vector3(0f, -12f, 1f);
        minigame.transform.DOLocalMove(Vector3.forward, 1f).SetEase(Ease.OutQuad).OnComplete(
            () => Time.timeScale = 0f);
        RPSMinigame game = minigame.GetComponent<RPSMinigame>();
        game.onWin.AddListener(MinigameWon);
        game.onLoseDraw.AddListener(MinigameLost);
        
    }
}
