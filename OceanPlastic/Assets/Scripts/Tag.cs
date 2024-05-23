using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour
{
    [SerializeField]
    private List<Tags> tags;
    
    public List<Tags> GetTags()
    {
        return tags;
    }
    
    public bool HasTag(Tags tag)
    {
        return tags.Contains(tag);
    }
}

public enum Tags
{
    Player,
    FloatingTrash,
    Terrain,
    Tool,
    Vacuum,
}
