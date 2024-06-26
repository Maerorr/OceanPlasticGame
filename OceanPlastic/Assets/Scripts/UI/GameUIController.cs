using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    private List<RectTransform> children;
    private List<Vector2> defaultPositions;
    public Image key;
    public int keysAmount;
    public TextMeshProUGUI keysAmountText;

    public GameObject moveJoystick;
    
    bool pauseMenuActive = false;
    
    private void Start()
    {
        keysAmountText.text = "";
        key.color = Color.clear;
        children = new List<RectTransform>();
        defaultPositions = new List<Vector2>();
        foreach (Transform child in transform)
        {
            children.Add(child.GetComponent<RectTransform>());
            defaultPositions.Add(child.GetComponent<RectTransform>().anchoredPosition);
        }
    }

    public void MoveAside()
    {
        // move all children aside in a center-object direction
        foreach (var child in children)
        {
            Vector2 direction = (child.position - transform.position).normalized;
            child.DOAnchorPos(child.anchoredPosition + direction * 800, 1f).SetEase(Ease.OutQuad).OnComplete(
                () =>
                    {
                        moveJoystick.SetActive(false);
                    }
                );
        }
    }

    public void MoveToNormal()
    {
        // move all children back to their default positions
        moveJoystick.SetActive(true);
        for (int i = 0; i < children.Count; i++)
        {
            children[i].DOAnchorPos(defaultPositions[i], 1f).SetEase(Ease.InQuad);
        }
    }

    public void UpdateKey(int addOrRemoveAmount)
    {
        keysAmount += addOrRemoveAmount;
        if (keysAmount > 0)
        {
            keysAmountText.text = $"{keysAmount} x";
            key.color = Color.white;
        }
        else
        {
            keysAmountText.text = "";
            key.color = Color.clear;
        }
    }
}
