using System;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [SerializeField] 
    private GameObject vacuum;
    [SerializeField] 
    private GameObject forceCannon;
    
    private CurrentTool currentTool = CurrentTool.None;

    private void Start()
    {
        UpdateCurrentTool(currentTool);
    }

    public void SetCurrentTool(CurrentTool tool)
    {
        currentTool = tool;
        UpdateCurrentTool(tool);
    }

    private void UpdateCurrentTool(CurrentTool tool)
    {
        switch (tool)
        {
            case CurrentTool.None:
                vacuum.SetActive(false);
                forceCannon.SetActive(false);
                break;
            case CurrentTool.Vacuum:
                vacuum.SetActive(true);
                forceCannon.SetActive(false);
                break;
            case CurrentTool.ForceCannon:
                vacuum.SetActive(false);
                forceCannon.SetActive(true);
                break;
        }
    
    }
}

[Serializable]
public enum CurrentTool
{
    None,
    Vacuum,
    ForceCannon
}