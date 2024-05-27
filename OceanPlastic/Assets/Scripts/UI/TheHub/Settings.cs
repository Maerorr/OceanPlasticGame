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
}
