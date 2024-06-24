using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class MinigameTrigger : MonoBehaviour
{
    public string buttonMessage = "Tap to play!";
    
    public GameObject minigamePrefab;
    private GameObject minigameInstance;

    private GameUIController gameUIController;
    private ToolButtons toolButtons;

    public UnityEvent onWin;
    public UnityEvent onMinigameHidden;
    public bool isPlayerInside = false;
    public bool hasPlayerWon = false;
    
    private FullScreenPostProcessController postprocess;

    public void MinigameTriggerInit()
    {
        toolButtons = FindObjectOfType<ToolButtons>();
        toolButtons.extraButton.onClick.AddListener(ShowMinigame);
        gameUIController = FindObjectOfType<GameUIController>();
        postprocess = FindObjectOfType<FullScreenPostProcessController>();
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
                toolButtons.SetTooltip(buttonMessage);
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
    
    private void HideMinigame()
    {
        Hide();
    }

    private void MinigameWon()
    {
        Hide();
        
        hasPlayerWon = true;
        onWin.Invoke();
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        postprocess.EnableRipples();
        gameUIController.MoveToNormal();
        minigameInstance.transform.DOLocalMove(new Vector3(0f, -12f, 5f), 1f).SetEase(Ease.OutQuad).OnComplete(
            () =>
            {
                Destroy(minigameInstance);
                onMinigameHidden.Invoke();
            }).SetUpdate(true);
    }

    public void ShowMinigame()
    {
        if (!isPlayerInside) return;
        if (hasPlayerWon) return;
        postprocess.DisableRipples();
        toolButtons.DisableExtraButton();
        toolButtons.ClearTooltip();
        gameUIController.MoveAside();
        minigameInstance = Instantiate(minigamePrefab, Vector3.zero, Quaternion.identity);
        minigameInstance.transform.parent = Camera.main.transform;
        minigameInstance.transform.localPosition = new Vector3(0f, -12f, 5f);
        minigameInstance.transform.DOLocalMove(new Vector3(0f, 0f, 5f), 1f).SetEase(Ease.OutQuad).OnComplete(
            () => Time.timeScale = 0f);
        Minigame minigame = minigameInstance.GetComponent<Minigame>();
        minigame.onWin.AddListener(MinigameWon);
        minigame.onBack.AddListener(HideMinigame);
    }
}
