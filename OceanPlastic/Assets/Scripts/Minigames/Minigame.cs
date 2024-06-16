using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent onWin;
    public UnityEvent onBack;

    protected void MinigameInit()
    {
        GetComponentInChildren<NonUIButton>().buttonClicked.AddListener(() =>
        {
            onBack.Invoke();
        });
    }
}
