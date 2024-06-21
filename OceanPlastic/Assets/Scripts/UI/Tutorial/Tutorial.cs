using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> tutorialSteps = new List<GameObject>();
    public TextMeshProUGUI tapAnywhereText;
    int currentStep = 0;
    Image thisPanel;
    
    void Start()
    {
        thisPanel = GetComponent<Image>();
        SetPanelActive(false);
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.SetActive(false);
        }
        StartCoroutine(StartTutorial());
    }
    
    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(2f);
        EnableNextStep();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        SetPanelActive(false);
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.SetActive(false);
        }
        StartCoroutine(DelayShowNextTip());
    }

    IEnumerator DelayShowNextTip()
    {
        yield return new WaitForSeconds(1f);
        EnableNextStep();
    }

    public void EnableNextStep()
    {
        SetPanelActive(true);
        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(true);
            currentStep++;
        }
    }

    private void SetPanelActive(bool active)
    {
        if (active)
        {
            thisPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.6f);
            thisPanel.raycastTarget = true;
            tapAnywhereText.color = Color.white;
        }
        else
        {
            thisPanel.color = Color.clear;
            thisPanel.raycastTarget = false;
            tapAnywhereText.color = Color.clear;
        }
    }
}
