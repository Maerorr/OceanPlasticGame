using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Settings : MonoBehaviour
{
    private bool active = true;
    public ScriptableRendererFeature feature;
    [SerializeField] 
    private TextMeshProUGUI postprocessCheck;
    [SerializeField] 
    private TextMeshProUGUI oxygenUsageCheck;
    public TextMeshProUGUI outlineCheck;
    private bool oxygenUsageActive = true;

    private void Start()
    {
        if (StaticGameData.instance.ripplePostProcess)
        {
            feature.SetActive(true);
            postprocessCheck.text = "";
        }
        else
        {
            feature.SetActive(false);
            postprocessCheck.text = "X";
        }

        if (StaticGameData.instance.showOutline)
        {
            outlineCheck.text = "X";
        }
        else
        {
            outlineCheck.text = "";
        }
        
        oxygenUsageCheck.text = StaticGameData.instance.oxygenUsage ? "" : "X";
        oxygenUsageActive = StaticGameData.instance.oxygenUsage;
    }

    public void TogglePostProcess()
    {
        if (active)
        {
            postprocessCheck.text = "X";
            feature.SetActive(false);
            active = false;
            StaticGameData.instance.ripplePostProcess = false;
        }
        else
        {
            postprocessCheck.text = "";
            feature.SetActive(true);
            active = true;
            StaticGameData.instance.ripplePostProcess = true;
        }
    }

    public void ToggleOxygenUsage()
    {
        if (oxygenUsageActive)
        {
            oxygenUsageCheck.text = "X";
            oxygenUsageActive = false;
            StaticGameData.instance.oxygenUsage = false;
        }
        else
        {
            oxygenUsageCheck.text = "";
            oxygenUsageActive = true;
            StaticGameData.instance.oxygenUsage = true;
        }
    }

    public void ToggleOutline()
    {
        if (outlineCheck.text == "X")
        {
            outlineCheck.text = "";
            StaticGameData.instance.showOutline = false;
        }
        else
        {
            outlineCheck.text = "X";
            StaticGameData.instance.showOutline = true;
        }
    }

    public void DemoAddMoney()
    {
        StaticGameData.instance.AddMoney(1000);
    }
}
