using System;
using System.Collections;
using DG.Tweening;
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
    [SerializeField, Range(1f, 500f)] 
    private float secondsOfOxygen;
    [SerializeField]
    private float oxygenDecreaseRate = 5f;
    [SerializeField]
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

    #endregion
    
    [Header("Health")]
    
    [SerializeField]
    Slider healthSlider;
    
    [SerializeField] 
    private float maxHealth = 100f;

    private float currentHealth;
    private bool canHeal = true;

    [SerializeField] 
    private float healPerSecond = 5f;

    private bool isDrowning = false;
    private bool isTooDeep = false;
    
    [SerializeField] 
    private int maxSafeDepth = 75;

    private int currentDepth = 0;

    private Messenger msg;

    public SpriteRenderer playerSprite;
    private int playerDamageTweenID;
    
    [Header("Events")]
    public UnityEvent onDeath;
    
    public UnityEvent onDamageTaken;

    #region Coroutines

    private Coroutine oxygenSliderCoroutine;
    private Coroutine oxygenTimerCoroutine;
    private Coroutine disableHealingCoroutine;
    private Coroutine damageCoroutine;

    #endregion
    
    void Start()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        
        msg ??= FindAnyObjectByType<Messenger>();
        oxygenDecreaseRate = baseMaxOxygen / secondsOfOxygen;
        
        currentHealth = maxHealth;
        oxygenSliderCoroutine = StartCoroutine(HandleOxygenSlider());
        oxygenTimerCoroutine = StartCoroutine(HandleOxygenTimer());
        damageCoroutine = StartCoroutine(DamageCoroutine());
        StartCoroutine(HandleDepth());
        oxygenDivisor = 1f / oxygeneUpdateRate;
    }

    private void Update()
    {
        if (canHeal)
        {
            currentHealth += healPerSecond * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
        
        UpdateHealthSlider();
    }

    // this updates the oxygen SLIDER every (oxygenUpdateRate) seconds
    IEnumerator HandleOxygenSlider()
    {
        while(true)
        {
            yield return new WaitForSeconds(oxygeneUpdateRate);
            // disable oxygen usage
            if (!StaticGameData.instance.oxygenUsage) canBreathe = true;
            if (!canBreathe)
            {
                UpdateOxygenSlider(-(oxygenDecreaseRate * oxygenDecreaseModifier) / oxygenDivisor);
            }
            else
            {
                UpdateOxygenSlider(20f);
            }
        }
    }
    
    IEnumerator HandleOxygenTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateOxygenTimer();
        }
    }

    IEnumerator HandleDepth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (currentDepth < -maxSafeDepth)
            {
                isTooDeep = true;
            }
            else
            {
                isTooDeep = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            onDeath.Invoke();
            StartCoroutine(DeathMessage());
        }
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        if (disableHealingCoroutine != null)
        {
            StopCoroutine(disableHealingCoroutine);
        }
        disableHealingCoroutine = StartCoroutine(DisableHealing());
        onDamageTaken.Invoke();
        DOTween.Kill(playerDamageTweenID);
        playerSprite.color = new Color(1f, 0.5f, 0.5f, 1f);
        playerDamageTweenID = playerSprite.DOColor(Color.white, 0.66f).SetEase(Ease.InCubic).SetId(5534).intId;
    }

    IEnumerator DeathMessage()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        msg.ShowMessage($"{PlayerManager.Instance.trashLostOnPreviousDeath} Trash lost!", transform.position, Color.red, 3f);
    }
    
    public void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    private void UpdateOxygenSlider(float delta)
    {
        currentOxygen += delta;
        if (currentOxygen <= 0f)
        {
            isDrowning = true;
        }
        else
        {
            isDrowning = false;
        }
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
        oxygenDecreaseModifier = 1f / decreaseModifier;
        oxygenIncreaseModifier = decreaseModifier;
    }
    
    private void UpdateHealthSlider()
    {
        healthSlider.value = currentHealth / maxHealth;
    }
    
    public void SetMaxSafeDepth(int depth)
    {
        maxSafeDepth = depth;
    }
    
    public float GetMaxSafeDepth()
    {
        return maxSafeDepth;
    }
    
    public void AddOxygen(float oxygenSeconds)
    {
        float secondsForEntireBar = (baseMaxOxygen + bonusMaxOxygen) / (oxygenDecreaseRate * oxygenDecreaseModifier);
        float addition = oxygenSeconds * (baseMaxOxygen + bonusMaxOxygen) / secondsForEntireBar;
        currentOxygen += addition;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, 100f);
        UpdateOxygenTimer();
    }

    IEnumerator DisableHealing()
    {
        canHeal = false;
        yield return new WaitForSeconds(1f);
        canHeal = true;
    }
    
    IEnumerator DamageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float damage = Convert.ToInt32(isDrowning) * 10f + Convert.ToInt32(isTooDeep) * 10f;
            if (damage < 0.1f) continue;
            TakeDamage(damage);
            onDamageTaken.Invoke();
        }
    }
}
