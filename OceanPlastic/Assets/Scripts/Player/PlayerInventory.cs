using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int maxCapacity = 5;
    private List<(FloatingTrashSO, int)> collectedTrash = new List<(FloatingTrashSO, int)>();
    private int currentCapacity = 0;
    
    private int money = 500;
    
    [SerializeField]
    private TrashMeter trashMeter;

    [SerializeField] 
    private MoneyCounter moneyCounter;

    [SerializeField] 
    private Messenger msg;
    
    private void Awake()
    {
        msg??= FindAnyObjectByType<Messenger>();
        trashMeter??= FindAnyObjectByType<TrashMeter>();
        moneyCounter??= FindAnyObjectByType<MoneyCounter>();
        trashMeter.SetTrashValue(currentCapacity, maxCapacity);
        UpdateMoneyCounter();
    }

    public bool AddItem(FloatingTrashSO item)
    {
        if ((currentCapacity + item.weight) > maxCapacity)
        {
            ShowMessage("Inventory is full!");
            return false;
        }
        
        if (collectedTrash.Count == 0)
        {
            collectedTrash.Add((item, 1));
        }
        else
        {
            var found = collectedTrash.Find(entry => entry.Item1.name == item.name);
            if (found.Item1 != null)
            {
                var index = collectedTrash.IndexOf(found);
                var newEntry = (found.Item1, found.Item2 + 1);
                collectedTrash[index] = newEntry;
            }
            else
            {
                collectedTrash.Add((item, 1));
            }
        }
        currentCapacity += item.weight;
        if (currentCapacity == maxCapacity)
        {
            ShowMessage("Inventory is full!");
        }
        UpdateTrashMeter();
        
        //Debug.Log($"new item added to inventory: {item.name}, this item count: {currentCount}, current capacity: {currentCapacity} / {maxCapacity}");
        return true;
    }

    public void RemoveTrash(FloatingTrashSO trash)
    {
        var found = collectedTrash.Find(entry => entry.Item1.name == trash.name);
        if (found.Item1 != null)
        {
            var index = collectedTrash.IndexOf(found);
            var newEntry = (found.Item1, found.Item2 - 1);
            collectedTrash[index] = newEntry;
            if (newEntry.Item2 == 0)
            {
                collectedTrash.RemoveAt(index);
            }
        }
        currentCapacity -= trash.weight;
        UpdateTrashMeter();
    }

    public void RemoveAllTrash()
    {
        collectedTrash.Clear();
        currentCapacity = 0;
        UpdateTrashMeter();
    }

    public void RemovePercentageOfTrash(float percentage)
    {
        int trashToRemove = Mathf.CeilToInt(collectedTrash.Sum(tuple => tuple.Item2) * percentage);
        Debug.Log("removing " + trashToRemove + " trash items");
        var trashArray = collectedTrash.ToArray();
        for (int i = 0; i < trashToRemove; i++)
        {
            RemoveTrash(trashArray[Random.Range(0, trashArray.Length)].Item1);
        }
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

    public List<(FloatingTrashSO, int)> GetInventory()
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
        UpdateTrashMeter();
    }

    private void ShowMessage(string message)
    {
        Vector3 msgPos = transform.position;
        msgPos.y += 1;
        msg.ShowMessage(message, msgPos, Color.red, 3f);
    }

    public int GetTrashCount()
    {
        return collectedTrash.Sum(tuple => tuple.Item2);
    }
}