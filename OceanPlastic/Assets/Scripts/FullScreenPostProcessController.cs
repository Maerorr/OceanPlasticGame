using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullScreenPostProcessController : MonoBehaviour
{
    [SerializeField] private float strength;
    [SerializeField] private float scale;
    [SerializeField] private float zoom;
    
    private int stregthID = Shader.PropertyToID("_Strength");
    private int scaleID = Shader.PropertyToID("_Scale");
    private int zoomID = Shader.PropertyToID("_Zoom");
    private int waterLevelID = Shader.PropertyToID("_WaterLevel");
    
    private int damageVignetteID = Shader.PropertyToID("_DamageIntensity");

    [SerializeField] private ScriptableRendererFeature waterRipple;
    [SerializeField] private Material waterRippleMaterial;
    
    private Camera cam;
    private float cameraHalfSize;
    
    public float damageVignetteTime = 1f;
    public float damageVignetteMaxIntensity = 0.5f;
    float damageVignetteIntensity = 0f;
    private int damageTween;

    private void Start()
    {
        cam = Camera.main;
        cameraHalfSize = cam.orthographicSize;
        waterRipple.SetActive(StaticGameData.instance.ripplePostProcess);
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
    }

    public void TriggerDamageVignette()
    {
        DOTween.Kill(damageTween);
        damageVignetteIntensity = damageVignetteMaxIntensity;
        damageTween = DOTween.To(() => damageVignetteIntensity, x => damageVignetteIntensity = x, 0f, damageVignetteTime).intId;
    }
}
