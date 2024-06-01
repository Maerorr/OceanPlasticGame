using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SellItemsList : MonoBehaviour
{
    private GameObject root;
    [SerializeField]
    private GameObject entriesParent;
    [SerializeField]
    private GameObject sellItemEntryPrefab;

    [SerializeField]
    private Vector2 openPosition;
    [SerializeField]
    private Vector2 closedPosition;
    
    private RectTransform rect;
    
    [SerializeField] 
    private UnityEvent onSell;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        root = transform.gameObject;
        rect.DOAnchorPos(closedPosition, 0.75f).SetEase(Ease.OutQuad).OnComplete(
            () => root.SetActive(false)
        );
    }
    
    public void ShowSellItemsList()
    {
        root.SetActive(true);
        rect.DOAnchorPos(openPosition, 0.75f).SetEase(Ease.OutQuad);
        PopulateSellItemsList();
    }
    
    public void HideSellItemsList()
    {
        rect.DOAnchorPos(closedPosition, 0.75f).SetEase(Ease.OutQuad).OnComplete(
            () => root.SetActive(false)
        );
    }

    public void SellItems()
    {
        onSell.Invoke();
        HideSellItemsList();
    }
    
    private void PopulateSellItemsList()
    {
        foreach (Transform child in entriesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var item in PlayerManager.Instance.PlayerInventory.GetInventory())
        {
            var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
            entry.transform.SetParent(entriesParent.transform);
            entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                item.Item1.spriteVariants[0], 
                item.Item1.name, 
                item.Item2, 
                item.Item1.value, 
                item.Item2 * item.Item1.value
                );
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(rect);
    }
}
