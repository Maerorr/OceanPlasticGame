using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{
    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    public TextMeshProUGUI tapAnywhereText;
    int currentStep = 0;
    Image thisPanel;

    public bool afterCollectingTrash = false;
    
    public GameObject toolButtons;
    public GameObject objectives;
    public GameObject armJoystick;
    public GameObject trashMeter;
    public GameObject tutorialTrash;
    public FloatingTrashSO bagSO;

    private bool alreadyGotMoney = false;
    
    void Start()
    {
        StaticGameData.instance.inTutorial = true;
        tutorialTrash.SetActive(false);
        toolButtons.SetActive(false);
        objectives.SetActive(false);
        armJoystick.SetActive(false);
        trashMeter.SetActive(false);
        thisPanel = GetComponent<Image>();
        SetPanelActive(false);
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.go.SetActive(false);
        }
        StartCoroutine(StartTutorial());
    }
    
    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1f);
        EnableNextStep();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        SetPanelActive(false);
        if (tutorialSteps[currentStep - 1].shouldTriggerNextStepAutomatically)
        {
            Debug.Log("Next Tip will spawn in delay seconds");
            StartCoroutine(DelayShowNextTip());
        }
        foreach (var tutorialStep in tutorialSteps)
        {
            tutorialStep.go.SetActive(false);
        }
    }

    IEnumerator DelayShowNextTip()
    {
        yield return new WaitForSecondsRealtime(1f);
        EnableNextStep();
    }

    public void EnableNextStep()
    {
        if (currentStep < tutorialSteps.Count)
        {
            SetPanelActive(true);
            tutorialSteps[currentStep].go.SetActive(true);
            currentStep++;
        }
    }

    private void SetPanelActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 0f;
            thisPanel.color = new Color(0.2f, 0.2f, 0.2f, 0.6f);
            thisPanel.raycastTarget = true;
            tapAnywhereText.color = Color.white;
        }
        else
        {
            Time.timeScale = 1f;
            thisPanel.color = Color.clear;
            thisPanel.raycastTarget = false;
            tapAnywhereText.color = Color.clear;
        }
    }

    public void ShowTools()
    {
        toolButtons.SetActive(true);
        armJoystick.SetActive(true);
        tutorialTrash.SetActive(true);
    }

    public void ShowTrashMeter()
    {
        trashMeter.SetActive(true);
        afterCollectingTrash = true;
    }

    public void ShowObjectives()
    {
        objectives.SetActive(true);
        FindAnyObjectByType<LevelManager>().AddCollectionObjective(bagSO);
        objectives.GetComponent<Objectives>().AddTrashObjective(bagSO, 1);
    }

    public void DisplayExitLevelTip()
    {
        EnableNextStep();
    }

    public void OnTutorialFinished()
    {
        if (alreadyGotMoney) return;
        StaticGameData.instance.AddMoney(50);
        StaticGameData.instance.inTutorial = false;
        alreadyGotMoney = true;
    }
}

[Serializable]
public struct TutorialStep
{
    public GameObject go;
    public bool shouldTriggerNextStepAutomatically;
}
