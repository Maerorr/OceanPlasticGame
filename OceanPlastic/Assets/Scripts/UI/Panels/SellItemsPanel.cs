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
    private UnityEvent onSell;

    private void Awake()
    {
        root = transform.gameObject;
        root.SetActive(false);
    }
    
    public void ShowSellItemsList()
    {
        root.SetActive(true);
        PopulateSellItemsList();
    }
    
    public void HideSellItemsList()
    {
        root.SetActive(false);
    }

    public void SellItems()
    {
        onSell.Invoke();
        HideSellItemsList();
    }
    
    private void PopulateSellItemsList()
    {
        // clear
        foreach (Transform child in entriesParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        // populate
        foreach (var item in PlayerManager.Instance.PlayerInventory.GetInventory())
        {
            var entry = Instantiate(sellItemEntryPrefab, entriesParent.transform);
            // assign parent as 'root'
            entry.transform.SetParent(entriesParent.transform);
            entry.GetComponent<SellItemEntry>().SetItemEntryValues(
                item.Item1.spriteVariants[0], 
                item.Item1.name, 
                item.Item2, 
                item.Item1.value, 
                item.Item2 * item.Item1.value);
        }
    }
}
