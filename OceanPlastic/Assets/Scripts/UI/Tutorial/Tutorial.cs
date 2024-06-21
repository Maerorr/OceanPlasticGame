using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> tutorialSteps = new List<GameObject>();
    int currentStep = 0;
    Image thisPanel;
    
    void Start()
    {
        thisPanel = GetComponent<Image>();
        thisPanel.color = Color.clear;
        thisPanel.raycastTarget = false;
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
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.SetActive(false);
            thisPanel.color = Color.clear;
            thisPanel.raycastTarget = false;
        }
    }

    public void EnableNextStep()
    {
        thisPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.6f);
        thisPanel.raycastTarget = true;
        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(true);
            currentStep++;
        }
    }
}
