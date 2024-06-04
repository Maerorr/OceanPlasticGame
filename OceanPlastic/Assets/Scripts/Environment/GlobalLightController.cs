using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerFlashlight;
    
    public float depthOfMaxDarkness = 150f;
    public float depthOfMaxFlashlight = 75f;
    
    public AnimationCurve globalLightIntensityCurve;
    public AnimationCurve playerFlashlightIntensityCurve;

    private void Update()
    {
        //Lower the globallight intensity based on players Y position from 0 to -150
        float playerY = Mathf.Min(playerFlashlight.transform.position.y, 0f);
        // globalLight.intensity = Mathf.Clamp01(1 + playerY / 150f);
        // // do the opposite for the flashlight
        // playerFlashlight.intensity = Mathf.Clamp01(-playerY / 75f);
        globalLight.intensity = globalLightIntensityCurve.Evaluate(- playerY / depthOfMaxDarkness);
        playerFlashlight.intensity = playerFlashlightIntensityCurve.Evaluate(-playerY / depthOfMaxFlashlight);
    }
}
