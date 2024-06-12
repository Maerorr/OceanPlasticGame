using System;
using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [SerializeField] 
    private GameObject vacuum;
    [SerializeField] 
    private GameObject forceCannon;

    private Vacuum vacuumScript;
    
    private PlayerAnimation anim;
    
    private CurrentTool currentTool = CurrentTool.None;

    private void Start()
    {
        vacuumScript = vacuum.GetComponent<Vacuum>();
        anim = FindAnyObjectByType<PlayerAnimation>();
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
                anim.PlayerNoToolSpriteSheet();
                break;
            case CurrentTool.Vacuum:
                vacuum.SetActive(true);
                forceCannon.SetActive(false);
                anim.PlayerToolSpriteSheet();
                break;
            case CurrentTool.ForceCannon:
                vacuum.SetActive(false);
                forceCannon.SetActive(true);
                anim.PlayerToolSpriteSheet();
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