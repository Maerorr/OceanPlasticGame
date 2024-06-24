using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarnaclePopper : Minigame
{
    private List<Barnacle> barnacles = new List<Barnacle>();
    
    private void Start()
    {
        MinigameInit();
        foreach (Barnacle barnacle in GetComponentsInChildren<Barnacle>())
        {
            barnacle.barnaclePopper = this;
            barnacles.Add(barnacle);
        }
    }
    
    public void PopBarnacle()
    {
        CheckBarnacles();
    }

    private void CheckBarnacles()
    {
        foreach (var barnacle in barnacles)
        {
            if (!barnacle.isPopped)
            {
                return;
            }
        }
        onWin.Invoke();
    }
}
