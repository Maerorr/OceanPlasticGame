using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    [SerializeField]
    GameObject objectiveEntryPrefab;
    
    [SerializeField]
    GameObject layoutRoot;
    
    private List<ObjectiveEntry> objectiveEntries = new List<ObjectiveEntry>();
    
    public void AddObjective(FloatingTrashSO trash, int amount)
    {
        var newObjective = Instantiate(objectiveEntryPrefab, layoutRoot.transform).GetComponent<ObjectiveEntry>();
        newObjective.SetSprite(trash.spriteVariants[0]);
        newObjective.SetObjectiveText($"{trash.name} Collected");
        newObjective.SetMaxValue(amount);
        newObjective.SetTrashName(trash.name);
        objectiveEntries.Add(newObjective);
    }

    public bool UpdateTrashObjectives(FloatingTrashSO collectedTrash)
    {
        var entry = objectiveEntries.Find(entry => entry.GetTrashName() == collectedTrash.name);
        entry.AddProgress(1);
        // return if is completed
        return entry.currentValue >= entry.maxValue;
    }
}
