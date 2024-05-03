using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float baseMaxOxygen = 100f;
    private float bonusMaxOxygen = 0f;
    
    private float currentOxygen = 100f;
    private float oxygenDecreaseRate = 5f;
    private float oxygenIncreaseRate = 20f;

    private bool isInWater = true;

    [SerializeField, Range(0.01f, 1f)] 
    private float oxygeneUpdateRate;

    private float oxygenDivisor;
    
    [SerializeField]
    private OxygenMeter oxygenMeter;
    private Coroutine oxygenSliderCoroutine;
    private Coroutine oxygenTimerCoroutine;

    void Start()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        oxygenSliderCoroutine = StartCoroutine(HandleOxygenSlider());
        oxygenTimerCoroutine = StartCoroutine(HandleOxygenTimer());
        
        oxygenDivisor = 1f / oxygeneUpdateRate;
    }

    IEnumerator HandleOxygenSlider()
    {
        while(true)
        {
            yield return new WaitForSeconds(oxygeneUpdateRate);
            if (isInWater)
            {
                UpdateOxygenSlider(-oxygenDecreaseRate / oxygenDivisor);
            }
            else
            {
                UpdateOxygenSlider(oxygenIncreaseRate / oxygenDivisor);
            }
        }
    }

    IEnumerator HandleOxygenTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (isInWater)
            {
                UpdateOxygenTimer();
            }
            else
            {
                UpdateOxygenTimer();
            }
        }
    }

    private void UpdateOxygenSlider(float delta)
    {
        currentOxygen += delta;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, 100f);
        oxygenMeter.SetSliderValue(currentOxygen);
    }
    
    private void UpdateOxygenTimer()
    {
        var secondsForEntireBar = (baseMaxOxygen + bonusMaxOxygen) / oxygenDecreaseRate;
        var secondsLeft = currentOxygen / (baseMaxOxygen + bonusMaxOxygen) * secondsForEntireBar;
        oxygenMeter.SetTimerValue(Mathf.FloorToInt(secondsLeft));
    }
    
    public void SetIsInWater(bool value)
    {
        isInWater = value;
    }

    public bool IsInWater()
    {
        return isInWater;
    }
}
