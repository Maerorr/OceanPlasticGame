using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Oxygen

    [Header("Oxygen")]
    
    private float baseMaxOxygen = 100f;
    private float bonusMaxOxygen = 0f;
    
    private float currentOxygen = 100f;
    [SerializeField, Range(1f, 10f)]
    private float oxygenDecreaseRate = 5f;
    private float oxygenIncreaseRate = 20f;
    
    [SerializeField]
    private float oxygenDecreaseModifier = 1f;
    [SerializeField]
    private float oxygenIncreaseModifier = 1f;

    private bool isInWater = true;
    private bool canBreathe = false;

    [SerializeField, Range(0.01f, 1f)] 
    private float oxygeneUpdateRate;

    private float oxygenDivisor;
    
    [SerializeField]
    private OxygenMeter oxygenMeter;
    private Coroutine oxygenSliderCoroutine;
    private Coroutine oxygenTimerCoroutine;

    #endregion
    
    [Header("Health")]
    
    [SerializeField] 
    private float maxHealth = 100f;

    private float currentHealth;

    [SerializeField] 
    private float healPerSecond = 5f;

    [SerializeField] 
    private int maxSafeDepth = 75;

    private int currentDepth = 0;

    [Header("Events")]
    [SerializeField] 
    private UnityEvent onDeath;

    [SerializeField] 
    private UnityEvent onDamageTaken;
    
    void Start()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

        currentHealth = maxHealth;
        oxygenSliderCoroutine = StartCoroutine(HandleOxygenSlider());
        oxygenTimerCoroutine = StartCoroutine(HandleOxygenTimer());
        oxygenDivisor = 1f / oxygeneUpdateRate;
    }

    private void Update()
    {
        currentHealth += healPerSecond * Time.deltaTime;
    }

    // this updates the oxygen SLIDER every (oxygenUpdateRate) seconds
    IEnumerator HandleOxygenSlider()
    {
        while(true)
        {
            yield return new WaitForSeconds(oxygeneUpdateRate);
            if (!canBreathe)
            {
                UpdateOxygenSlider(-(oxygenDecreaseRate * oxygenDecreaseModifier) / oxygenDivisor);
                if (currentOxygen <= 0f)
                {
                    onDeath.Invoke();
                }
            }
            else
            {
                UpdateOxygenSlider((oxygenIncreaseRate * oxygenIncreaseModifier) / oxygenDivisor);
            }
        }
    }

    // this updates the oxygen timer TEXT VALUE every second
    IEnumerator HandleOxygenTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateOxygenTimer();
        }
    }

    IEnumerator HandleDepth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (currentDepth < maxSafeDepth)
            {
                TakeDamage();
            }
        }
    }

    private void TakeDamage()
    {
        onDamageTaken.Invoke();
    }

    private void UpdateOxygenSlider(float delta)
    {
        currentOxygen += delta;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, 100f);
        oxygenMeter.SetSliderValue(currentOxygen);
    }
    
    private void UpdateOxygenTimer()
    {
        var secondsForEntireBar = (baseMaxOxygen + bonusMaxOxygen) / (oxygenDecreaseRate * oxygenDecreaseModifier);
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

    public void SetCanBreathe(bool canBreathe)
    {
        this.canBreathe = canBreathe;
    }
    
    public bool CanBreathe()
    {
        return canBreathe;
    }

    public void SetCurrentDepth(int depth)
    {
        currentDepth = depth;
    }
    
    public void SetOxygenModifier(float decreaseModifier)
    {
        oxygenDecreaseModifier = decreaseModifier;
        oxygenIncreaseModifier = 1f / decreaseModifier;
    }
}
