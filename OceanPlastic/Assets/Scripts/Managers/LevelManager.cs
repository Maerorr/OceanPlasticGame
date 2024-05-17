using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    // (trash type, amount, isCompleted)
    private List<(FloatingTrashSO, int, bool)> collectionObjective;
    int allTrashCount;
    
    private Objectives objectives;

    private void Start()
    {
        FindAnyObjectByType<BoatSellZone>().onSellTrash.AddListener(TrashCollected);
        
        objectives = FindObjectOfType<Objectives>();
        var allTrash = FindObjectsOfType<FloatingTrash>();
        allTrashCount = allTrash.Length;
        // get all unique trash types
        var trashTypes = new HashSet<FloatingTrashSO>();
        foreach (var trash in allTrash)
        {
            trashTypes.Add(trash.GetData());
        }
        collectionObjective = new List<(FloatingTrashSO, int, bool)>();
        foreach (var trashType in trashTypes)
        {
            int allTrashOfType = allTrash.Sum(trash => trash.GetData().name == trashType.name ? 1 : 0);
            collectionObjective.Add(
                (trashType, Random.Range(
                    allTrashOfType / 2,
                    Mathf.FloorToInt(allTrashOfType * 0.9f)),
                    false
                )
            );
        }

        foreach (var obj in collectionObjective)
        {
            objectives.AddObjective(obj.Item1, obj.Item2);
        }
    }

    public void TrashCollected(FloatingTrashSO trash)
    {
        bool isCompleted = objectives.UpdateTrashObjectives(trash);
        if (isCompleted)
        {
            var entry = collectionObjective.Find(entry => entry.Item1.name == trash.name);
            entry.Item3 = true;
        }
    }
}