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
    public GameObject outlineCheck;

    public Image panel;

    private OutlineShadersController outlineController;
    
    public Material waterOverlayMaterial;
    private int disableOverlayID = Shader.PropertyToID("_DisableOverlay");
    
    bool isOpen = false;
    
    private void Start()
    {
        outlineController = FindAnyObjectByType<OutlineShadersController>();
        if (feature.isActive)
        {
            rippleCheckmark.SetActive(false);
        }
        else
        {
            rippleCheckmark.SetActive(true);
        }

        if (StaticGameData.instance.showOutline)
        {
            outlineCheck.SetActive(true);
        }
        else
        {
            outlineCheck.SetActive(false);
        }
        panel.raycastTarget = false;

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
        Time.timeScale = 1;
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
            panel.raycastTarget = false;
            isOpen = false;
            Time.timeScale = 1;
        }
        else
        {
            pauseMenuRoot.color = initialColor;
            pauseMenuChild.SetActive(true);
            panel.raycastTarget = true;
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
        
        if (StaticGameData.instance.showOutline)
        {
            outlineCheck.SetActive(true);
        }
        else
        {
            outlineCheck.SetActive(false);
        }
    }

    public void OnRippleChange()
    {
        if (feature.isActive)
        {
            feature.SetActive(false);
            StaticGameData.instance.ripplePostProcess = false;
            rippleCheckmark.SetActive(true);
            Debug.Log("setting overlay material to 0");
            waterOverlayMaterial.SetFloat(disableOverlayID, 1.5f);
        }
        else
        {
            feature.SetActive(true);
            StaticGameData.instance.ripplePostProcess = true;
            rippleCheckmark.SetActive(false);
            Debug.Log("setting overlay material to 1");
            waterOverlayMaterial.SetFloat(disableOverlayID, 0f);
        }
    }

    public void OnOutlineChange()
    {
        if (StaticGameData.instance.showOutline)
        {
            outlineCheck.SetActive(false);
            StaticGameData.instance.showOutline = false;
            outlineController.DisableOutline();
        }
        else
        {
            outlineCheck.SetActive(true);
            StaticGameData.instance.showOutline = true;
            outlineController.EnableOutline();
        }
    }
}
