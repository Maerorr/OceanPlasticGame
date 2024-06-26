using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class FullScreenPostProcessController : MonoBehaviour
{
    [SerializeField] private float strength;
    [SerializeField] private float scale;
    [SerializeField] private float zoom;
    
    // private int stregthID = Shader.PropertyToID("_Strength");
    // private int scaleID = Shader.PropertyToID("_Scale");
    // private int zoomID = Shader.PropertyToID("_Zoom");
    private int waterLevelID = Shader.PropertyToID("_WaterLevel");
    
    private int damageVignetteID = Shader.PropertyToID("_DamageIntensity");

    [SerializeField] private ScriptableRendererFeature waterRipple;
    [SerializeField] private Material waterRippleMaterial;
    
    public Material waterOverlayMaterial;
    private int disableOverlayID = Shader.PropertyToID("_DisableRipples");
    private int totalUnscaledTimeID = Shader.PropertyToID("_TotalUnscaledTime");
    private float totalUnscaledTime = 0;
    
    private Camera cam;
    private float cameraHalfSize;
    private float initialStrength;
    
    public float damageVignetteTime = 1f;
    public float damageVignetteMaxIntensity = 0.5f;
    float damageVignetteIntensity = 0f;
    private int damageTween;

    private void Start()
    {
        cam = Camera.main;
        cameraHalfSize = cam.orthographicSize;
        waterRipple.SetActive(StaticGameData.instance.ripplePostProcess);
        if (StaticGameData.instance.ripplePostProcess)
        {
            waterOverlayMaterial.SetFloat(disableOverlayID, 0f);
        }
        else
        {
            waterOverlayMaterial.SetFloat(disableOverlayID, 1.5f);
        }
        initialStrength = strength;
    }

    private void Update()
    {
        var camBottom = cam.transform.position.y - cameraHalfSize;
        if (camBottom > 0f)
        {
            waterRippleMaterial.SetFloat(waterLevelID, 1f);
        }
        var waterLevel = 1f - (-camBottom / (2f * cameraHalfSize));
        waterRippleMaterial.SetFloat(waterLevelID, waterLevel);
        waterRippleMaterial.SetFloat(damageVignetteID, damageVignetteIntensity);
        waterOverlayMaterial.SetFloat(totalUnscaledTimeID, Time.unscaledTime);
    }

    public void DisableRipples()
    {
        // var currentStrength = initialStrength;
        // DOTween.To(() => currentStrength, x => currentStrength = x, 0f, 1f).OnUpdate(() =>
        // {
        //     waterRippleMaterial.SetFloat(stregthID, currentStrength);
        // }).SetUpdate(true);
        waterRipple.SetActive(false);
        //waterOverlayMaterial.SetInt(disableOverlayID, 0);
    }

    public void EnableRipples()
    {
        waterRipple.SetActive(true);
        //waterOverlayMaterial.SetInt(disableOverlayID, 1);
        // var currentStrength = 0f;
        // if (StaticGameData.instance.ripplePostProcess)
        // {
        //     DOTween.To(() => currentStrength, x => currentStrength = x, initialStrength, 1f).OnUpdate(() =>
        //     {
        //         waterRippleMaterial.SetFloat(stregthID, currentStrength);
        //     }).SetUpdate(true);
        // }
    }

    public void TriggerDamageVignette()
    {
        DOTween.Kill(damageTween);
        damageVignetteIntensity = damageVignetteMaxIntensity;
        damageTween = DOTween.To(() => damageVignetteIntensity, x => damageVignetteIntensity = x, 0f, damageVignetteTime).SetId(7836945).intId;
    }
}
