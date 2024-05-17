using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<(FloatingTrashSO, int)> collectionObjective;

    private void Awake()
    {
        collectionObjective = new List<(FloatingTrashSO, int)>();
        
    }

    public void AddCollectionObjective(FloatingTrashSO trash, int amount)
    {
        collectionObjective.Add((trash, amount));
    }
}