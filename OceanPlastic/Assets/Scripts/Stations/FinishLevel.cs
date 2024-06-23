using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishLevel : MonoBehaviour
{
    public UnityEvent onFinishLevel;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var pi = other.GetComponentInParent<Player>();
        if (pi != null)
        {
            onFinishLevel.Invoke();
        }
    }
}
