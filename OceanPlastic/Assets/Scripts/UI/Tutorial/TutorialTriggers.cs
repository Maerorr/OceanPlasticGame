using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggers : MonoBehaviour
{
    public UnityEvent onTrigger;
    public bool afterTrash;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (afterTrash)
        {
            if (!FindAnyObjectByType<Tutorial>().afterCollectingTrash) return;
        }
        FindAnyObjectByType<Tutorial>().EnableNextStep();
        onTrigger.Invoke();
        Destroy(gameObject);
    }
}
