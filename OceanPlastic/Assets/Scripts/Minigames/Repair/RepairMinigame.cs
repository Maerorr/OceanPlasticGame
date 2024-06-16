using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepairMinigame : Minigame
{
    List<TouchDetectSquare> squares = new List<TouchDetectSquare>();
    public SpriteRenderer background;
    
    private void Start()
    {
        foreach (var square in GetComponentsInChildren<TouchDetectSquare>())
        {
            square.onTouched.AddListener(IsAllSquaresTouched);
            squares.Add(square);
        }
        onWin.AddListener(ChangeBackground);
        
        MinigameInit();
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
        onWin.Invoke();
    }
    
    public void ChangeBackground()
    {
        background.color = Color.green;
    }
}
