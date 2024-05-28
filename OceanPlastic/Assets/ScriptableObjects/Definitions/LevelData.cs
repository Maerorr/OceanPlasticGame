using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string LevelPath;
    public List<Objective> objectives;
    public bool completed;
}

[Serializable]
public class Objective
{
    public ObjectiveType type;
    public FloatingTrashSO trash;
}

[Serializable]
public enum ObjectiveType
{
    Trash,
    Fix
}
