using UnityEngine;

[CreateAssetMenu(fileName = "FloatingTrash", menuName = "ScriptableObjects/FloatingTrashSO", order = 1)]
public class FloatingTrashSO : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public int weight;
    public int value;
}
