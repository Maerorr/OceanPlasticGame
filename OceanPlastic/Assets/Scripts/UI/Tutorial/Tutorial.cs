using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{
    List<GameObject> tutorialSteps = new List<GameObject>();
    int currentStep = 0;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.SetActive(false);
        }
    }

    public void EnableNextStep()
    {
        if (currentStep < tutorialSteps.Count)
        {
            tutorialSteps[currentStep].SetActive(true);
            currentStep++;
        }
    }
}
