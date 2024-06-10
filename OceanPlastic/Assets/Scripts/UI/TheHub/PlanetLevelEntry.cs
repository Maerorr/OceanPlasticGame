using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetLevelEntry : MonoBehaviour
{
    [SerializeField]
    private Image highlight;
    [SerializeField]
    private RectTransform planetInfoPanel;

    private int id;
    
    [SerializeField]
    private string scene;

    private bool active = false;
    private List<int> tweens = new List<int>();

    [SerializeField] 
    private float animationTime = 0.4f;
    
    private float moveY;
    private Vector2 initialScale;

    private PlanetEntryManager planetEntryManager;
    
    private SceneTransitionEffect sceneTransitionEffect;
    
    private void Start()
    {
        highlight.color = new Color(1f, 1f, 1f, 0f);
        active = false;
        planetInfoPanel.DOAnchorPos(new Vector2(0f, 0f), 0.0f);
        initialScale = planetInfoPanel.transform.localScale;
        planetInfoPanel.transform.localScale = new Vector3(0f, 0f, 0f);
        moveY = planetInfoPanel.anchoredPosition.y;
        planetEntryManager = FindObjectOfType<PlanetEntryManager>();
        sceneTransitionEffect = FindObjectOfType<SceneTransitionEffect>();
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    public int GetID()
    {
        return id;
    }
    
    public void OnPress()
    {
        planetEntryManager.OnPlanetPress(this);
        if (active)
        {
            Disable();
            return;
        }
        Enable();
    }

    public void Disable()
    {
        active = false;
        foreach (var tween in tweens)
        {
            DOTween.Kill(tween);
        }
        tweens.Clear();
        var colorTween = highlight.DOColor(new Color(1f, 1f, 1f, 0f), animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()).intId;
        var moveYTween = planetInfoPanel.DOAnchorPos(new Vector2(0f, 0f), animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()+1).intId;
        var scaleYTween = planetInfoPanel.transform.DOScaleY(0f, animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()+2).intId;
        var scaleXTween = planetInfoPanel.transform.DOScaleX(0f, animationTime).SetEase(Ease.OutCubic).OnComplete(
            () => { planetInfoPanel.gameObject.SetActive(false); Debug.Log("Setting planet to DISABLED"); }
        ).SetId(gameObject.GetInstanceID()+3).intId;
        
        tweens.Add(colorTween);
        tweens.Add(moveYTween);
        tweens.Add(scaleYTween);
        tweens.Add(scaleXTween);
    }

    public void Enable()
    {
        active = true;
        Debug.Log("Setting planet to ACTIVE");
        planetInfoPanel.gameObject.SetActive(true);
        foreach (var tween in tweens)
        {
            DOTween.Kill(tween);
        }
        tweens.Clear();
        //var moveYTween = planetInfoPanel.transform.DOMoveY(130f, 0.2f).SetEase(Ease.OutCubic);
        var colorTween = highlight.DOColor(new Color(1f, 1f, 1f, 0.3f), animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()).intId;
        var moveYTween = planetInfoPanel.DOAnchorPos(new Vector2(0f, moveY), animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()+1).intId;
        Debug.Log(initialScale);
        var scaleYTween = planetInfoPanel.transform.DOScaleY(initialScale.y, animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()+2).intId;
        var scaleXTween = planetInfoPanel.transform.DOScaleX(initialScale.x, animationTime).SetEase(Ease.OutCubic).SetId(gameObject.GetInstanceID()+3).intId;
        
        tweens.Add(colorTween);
        tweens.Add(moveYTween);
        tweens.Add(scaleYTween);
        tweens.Add(scaleXTween);
    }

    public void OnStart()
    {
        //SceneManager.LoadScene(scene);
        sceneTransitionEffect.FromHubToLevel(scene);
    }
}
