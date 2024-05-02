using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tags
{
    Player,
    FloatingTrash,
}

public class Tag : MonoBehaviour
{
    [SerializeField]
    private Tags tag;
    
    public Tags GetTag()
    {
        return tag;
    }
}
