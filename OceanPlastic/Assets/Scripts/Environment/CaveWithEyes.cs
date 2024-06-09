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
    public GameObject minigameInstance;

    public Transform key;
    
    private void Start()
    {
        toolButtons = FindObjectOfType<ToolButtons>();
        gameUIController = FindObjectOfType<GameUIController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        key.gameObject.SetActive(false);
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
        gameUIController.MoveToNormal();
        minigameInstance.transform.DOLocalMove(new Vector3(0f, -12f, 5f), 1f).SetEase(Ease.OutQuad).OnComplete(
            () =>
            {
                Time.timeScale = 1f;
                Destroy(minigameInstance);
                key.gameObject.SetActive(true);
                key.DOLocalMoveY(3f, 1.5f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(
                    () =>
                    {
                        key.GetComponent<Key>().StartFloating();
                    });
                key.DOScale(1f, 1f).SetEase(Ease.OutQuad).SetUpdate(true);
            }).SetUpdate(true);
        
        hasPlayerWon = true;
        //PlayerManager.Instance.PlayerInventory.SetHasKey(true);
    }

    private void MinigameLost()
    {
        gameUIController.MoveToNormal();
        minigameInstance.transform.DOLocalMove(new Vector3(0f, -12f, 5f), 1f).SetEase(Ease.OutQuad).OnComplete(
            () =>
            {
                Time.timeScale = 1f;
                Destroy(minigameInstance);
            }).SetUpdate(true);
    }

    public void StartMinigame()
    {
        if (!isPlayerInside) return;
        if (hasPlayerWon) return;
        toolButtons.DisableExtraButton();
        toolButtons.ClearTooltip();
        gameUIController.MoveAside();
        minigameInstance = Instantiate(minigamePrefab, Vector3.zero, Quaternion.identity);
        minigameInstance.transform.parent = Camera.main.transform;
        minigameInstance.transform.localPosition = new Vector3(0f, -12f, 5f);
        minigameInstance.transform.DOLocalMove(new Vector3(0f, 0f, 5f), 1f).SetEase(Ease.OutQuad).OnComplete(
            () => Time.timeScale = 0f);
        RPSMinigame game = minigameInstance.GetComponent<RPSMinigame>();
        game.onWin.AddListener(MinigameWon);
        game.onLoseDraw.AddListener(MinigameLost);
        
    }
}
