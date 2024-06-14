using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDescription", menuName = "ScriptableObjects/LevelDescription", order = 1)]
public class LevelDescription : ScriptableObject
{
    public string name;
    public string description;
    public string difficulty;
}
