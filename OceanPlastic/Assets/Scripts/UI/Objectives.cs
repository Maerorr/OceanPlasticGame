using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    [SerializeField]
    GameObject objectiveEntryPrefab;

    [SerializeField] 
    private Sprite pipeSprite;
    
    [SerializeField]
    GameObject layoutRoot;
    
    [HideInInspector]
    public List<ObjectiveEntry> objectiveEntries = new List<ObjectiveEntry>();
    
    public void AddTrashObjective(FloatingTrashSO trash, int amount)
    {
        var newObjective = Instantiate(objectiveEntryPrefab, layoutRoot.transform).GetComponent<ObjectiveEntry>();
        newObjective.SetSprite(trash.spriteVariants[0]);
        newObjective.SetObjectiveText($"{trash.materialType} Trash Collected");
        newObjective.SetEntryType(ObjectiveEntryType.Trash);
        newObjective.SetTrashColor(TrashColor.MaterialToColor(trash.materialType));
        newObjective.SetMaxValue(amount);
        newObjective.SetTrashName(trash.name);
        newObjective.materialType = trash.materialType;
        objectiveEntries.Add(newObjective);
    }
    
    public void AddRepairObjectives(int amountOfPipes)
    {
        var newObjective = Instantiate(objectiveEntryPrefab, layoutRoot.transform).GetComponent<ObjectiveEntry>();
        newObjective.SetSprite(pipeSprite);
        newObjective.SetTrashColor(new Color(0.1f, 0.1f, 0.1f));
        newObjective.SetEntryType(ObjectiveEntryType.Pipe);
        newObjective.SetObjectiveText("Pipes Repaired");
        newObjective.SetMaxValue(amountOfPipes);
        objectiveEntries.Add(newObjective);
    }

    public bool UpdateTrashObjectives(FloatingTrashSO collectedTrash)
    {
        var entry = objectiveEntries.Find(entry => entry.materialType == collectedTrash.materialType);
        entry.AddProgress(1);
   
        return entry.currentValue >= entry.maxValue;
    }

    public void UpdateRepairObjective()
    {
        var entry = objectiveEntries.Find(entry => entry.GetEntryType() == ObjectiveEntryType.Pipe);
        entry.AddProgress(1);
    }
}
