using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepairMinigame : MonoBehaviour
{
    List<TouchDetectSquare> squares = new List<TouchDetectSquare>();
    public UnityEvent onFinished;
    public SpriteRenderer background;
    
    private void Start()
    {
        foreach (var square in GetComponentsInChildren<TouchDetectSquare>())
        {
            square.onTouched.AddListener(IsAllSquaresTouched);
            squares.Add(square);
        }
        onFinished.AddListener(ChangeBackground);
    }
    
    public void IsAllSquaresTouched()
    {
        foreach (var square in squares)
        {
            if (!square.HasBeenTouched())
            {
                return;
            }
        }
        onFinished.Invoke();
    }
    
    public void ChangeBackground()
    {
        background.color = Color.green;
    }
}
