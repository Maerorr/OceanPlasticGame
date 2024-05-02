using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int maxCapacity = 5;
    private List<FloatingTrashSO> collectedTrash = new List<FloatingTrashSO>();
    private int currentCapacity = 0;
    
    [SerializeField]
    private TrashMeter trashMeter;

    public bool AddItem(FloatingTrashSO item)
    {
        if ((currentCapacity + item.weight) > maxCapacity)
        {
            Debug.Log("Inventory is full");
            return false;
        }
        collectedTrash.Add(item);
        currentCapacity += item.weight;
        trashMeter.SetTrashValue(currentCapacity, maxCapacity);
        
        Debug.Log($"new item added to inventory: {item.name}, current capacity: {currentCapacity} / {maxCapacity}");
        return true;
    }

    public void RemoveItem(FloatingTrashSO item)
    {
        collectedTrash.Remove(item);
    }

    public List<FloatingTrashSO> GetInventory()
    {
        return collectedTrash;
    }
}
