using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WaterShaderController : MonoBehaviour
{
    private bool active = true;
    public ScriptableRendererFeature feature;
    
    public void OnPress()
    {
        if (active)
        {
            feature.SetActive(false);
            active = false;
        }
        else
        {
            feature.SetActive(true);
            active = true;
        }
    }
}
