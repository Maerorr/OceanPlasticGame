using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Image pauseMenuRoot;
    public GameObject pauseMenuChild;
    public Color initialColor;
    
    public ScriptableRendererFeature feature;
    public GameObject rippleCheckmark;

    public GameObject abandonConfirmationPanel;
    
    bool isOpen = false;
    
    private void Start()
    {
        if (feature.isActive)
        {
            rippleCheckmark.SetActive(false);
        }
        else
        {
            rippleCheckmark.SetActive(true);
        }

        abandonConfirmationPanel.SetActive(false);
        initialColor = pauseMenuRoot.color;
        pauseMenuRoot.color = Color.clear;
        pauseMenuChild.SetActive(false);
    }

    public void OnResumePress()
    {
        OnBackPress();
    }

    public void OnAbandonPress()
    {
        abandonConfirmationPanel.SetActive(true);
    }

    public void OnAbandonConfirm()
    {
        SceneManager.LoadScene("TheHub");
    }

    public void OnAbandonCancel()
    {
        abandonConfirmationPanel.SetActive(false);
    }
    
    public void OnBackPress()
    {
        if (isOpen)
        {
            pauseMenuRoot.color = Color.clear;
            pauseMenuChild.SetActive(false);
            isOpen = false;
            Time.timeScale = 1;
        }
        else
        {
            pauseMenuRoot.color = initialColor;
            pauseMenuChild.SetActive(true);
            isOpen = true;
            Time.timeScale = 0;
        }
        
        if (feature.isActive)
        {
            rippleCheckmark.SetActive(false);
        }
        else
        {
            rippleCheckmark.SetActive(true);
        }
    }

    public void OnRippleChange()
    {
        Debug.Log($"ripple is {feature.isActive}");
        if (feature.isActive)
        {
            feature.SetActive(false);
            StaticGameData.instance.ripplePostProcess = false;
            rippleCheckmark.SetActive(true);
        }
        else
        {
            feature.SetActive(true);
            StaticGameData.instance.ripplePostProcess = true;
            rippleCheckmark.SetActive(false);
        }
    }
}
