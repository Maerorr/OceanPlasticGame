using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatingTrash", menuName = "ScriptableObjects/FloatingTrashSO", order = 1)]
public class FloatingTrashSO : ScriptableObject
{
    public string name;
    public List<Sprite> spriteVariants;
    public int weight;
    public int value;
}
