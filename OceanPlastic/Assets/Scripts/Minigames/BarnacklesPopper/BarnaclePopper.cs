using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarnaclePopper : MonoBehaviour
{
    private List<Barnacle> barnacles = new List<Barnacle>();

    public UnityEvent onWin;
    
    private void Start()
    {
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
        Debug.Log("All barnacles popped!");
        onWin.Invoke();
    }
}
