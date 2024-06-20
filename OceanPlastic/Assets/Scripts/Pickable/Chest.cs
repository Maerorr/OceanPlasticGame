using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite closedChest;
    public Sprite openChest;
    public SpriteRenderer spriteRenderer;
    
    public bool isOpen = false;
    private bool isPlayerNear = false;
    ToolButtons toolButtons;
    public Transform treasure;
    private GameUIController gameUIController;

    public Messenger msg;

    private void Start()
    {
        msg ??= FindAnyObjectByType<Messenger>();
        gameUIController = FindAnyObjectByType<GameUIController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = closedChest;
        toolButtons = FindAnyObjectByType<ToolButtons>();
        toolButtons.extraButton.onClick.AddListener(OpenChest);
        treasure.gameObject.SetActive(false);
    }

    public void OpenChest()
    {
        Debug.Log("Chest open triggered");
        if (!isOpen && isPlayerNear)
        {
            if (!PlayerManager.Instance.PlayerInventory.HasKey())
            {
                msg.ShowMessage("The chest is locked.", transform.position, Color.red, 3f);
                return;
            };
            isOpen = true;
            spriteRenderer.sprite = openChest;
            toolButtons.DisableExtraButton();
            toolButtons.ClearTooltip();
            treasure.gameObject.SetActive(true);
            treasure.DOLocalMoveY(3f, 3f).SetEase(Ease.OutQuart);
            treasure.DOScale(0.6f, 3f).SetEase(Ease.OutQuart);
            //hiddenImage.DOFade(0f, 6f).SetEase(Ease.InQuint).OnComplete(() => hiddenImage.gameObject.SetActive(false));
            gameUIController.UpdateKey(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return;
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerNear = true;
                toolButtons.EnableExtraButton();
                toolButtons.SetTooltip("Tap to open.");
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                isPlayerNear = false;
                toolButtons.DisableExtraButton();
                toolButtons.ClearTooltip();
            }
        }
    }
}
