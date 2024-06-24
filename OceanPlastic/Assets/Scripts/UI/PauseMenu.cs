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

    private OutlineShadersController outlineController;
    
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
