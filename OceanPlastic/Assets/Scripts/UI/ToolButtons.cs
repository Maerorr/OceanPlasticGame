using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButtons : MonoBehaviour
{
    [SerializeField]
    Button vacuumButton;
    [SerializeField]
    Button forceCannonButton;
    
    private PlayerTools playerTools;
    private CurrentTool currentTool = CurrentTool.None;

    private void Start()
    {
        playerTools = FindObjectOfType<PlayerTools>();
    }

    public void OnVacuumButtonClicked()
    {
        if (currentTool == CurrentTool.Vacuum)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
            playerTools.SetCurrentTool(currentTool);
        }
        else
        {
            currentTool = CurrentTool.Vacuum;
            vacuumButton.transform.GetComponent<Image>().color = Color.green;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
        }
        playerTools.SetCurrentTool(currentTool);
    }
    
    public void OnForceCannonButtonPressed()
    {
        if (currentTool == CurrentTool.ForceCannon)
        {
            currentTool = CurrentTool.None;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.white;
        }
        else
        {
            currentTool = CurrentTool.ForceCannon;
            vacuumButton.transform.GetComponent<Image>().color = Color.white;
            forceCannonButton.transform.GetComponent<Image>().color = Color.green;
        }
        playerTools.SetCurrentTool(currentTool);
    }
}
