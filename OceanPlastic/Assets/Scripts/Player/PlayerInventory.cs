using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int maxCapacity = 5;
    private List<FloatingTrashSO> collectedTrash = new List<FloatingTrashSO>();
    private int currentCapacity = 0;
    
    private int money = 0;
    
    [SerializeField]
    private TrashMeter trashMeter;

    [SerializeField] 
    private MoneyCounter moneyCounter;

    private void Awake()
    {
        trashMeter.SetTrashValue(currentCapacity, maxCapacity);
        UpdateMoneyCounter();
    }

    public bool AddItem(FloatingTrashSO item)
    {
        if ((currentCapacity + item.weight) > maxCapacity)
        {
            Debug.Log("Inventory is full");
            return false;
        }
        collectedTrash.Add(item);
        currentCapacity += item.weight;
        UpdateTrashMeter();
        
        Debug.Log($"new item added to inventory: {item.name}, current capacity: {currentCapacity} / {maxCapacity}");
        return true;
    }

    public void RemoveTrash(FloatingTrashSO trash)
    {
        collectedTrash.Remove(trash);
        currentCapacity -= trash.weight;
        UpdateTrashMeter();
    }

    public void RemoveAllTrash()
    {
        collectedTrash.Clear();
        currentCapacity = 0;
        UpdateTrashMeter();
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyCounter();
    }
    
    public bool RemoveMoney(int amount)
    {
        if (money - amount < 0)
        {
            return false;
        }
        money -= amount;
        UpdateMoneyCounter();
        return true;
    }

    public List<FloatingTrashSO> GetInventory()
    {
        return collectedTrash;
    }

    private void UpdateMoneyCounter()
    {
        moneyCounter.SetMoneyValue(money);
    }

    private void UpdateTrashMeter()
    {
        trashMeter.SetTrashValue(currentCapacity, maxCapacity);
    }
    
    public void SetBagModifier(int maxBagCapacity)
    {
        maxCapacity = maxBagCapacity;
    }
}
