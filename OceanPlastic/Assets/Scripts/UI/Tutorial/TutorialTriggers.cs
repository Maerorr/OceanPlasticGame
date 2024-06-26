using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggers : MonoBehaviour
{
    public UnityEvent onTrigger;
    public bool afterTrash;
    public bool onlyAfterDeposit;
    private bool afterDeposit = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (afterTrash)
        {
            if (!FindAnyObjectByType<Tutorial>().afterCollectingTrash) return;
        }
        if (onlyAfterDeposit)
        {
            if (!afterDeposit) return;
        }
        FindAnyObjectByType<Tutorial>().EnableNextStep();
        onTrigger.Invoke();
        Destroy(gameObject);
    }

    public void AfterDeposit()
    {
        afterDeposit = true;
    }
}
