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
    
    public void TogglePostProcess()
    {
        if (active)
        {
            postprocessCheck.text = "";
            feature.SetActive(false);
            active = false;
        }
        else
        {
            postprocessCheck.text = "X";
            feature.SetActive(true);
            active = true;
        }
    }
}
