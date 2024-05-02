using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OxygenMeter : MonoBehaviour
{
    [SerializeField] 
    Slider oxygenSlider;
    [SerializeField] 
    private TextMeshProUGUI percentageText;
    
    private int currentPercentage = 100;

    public void SetSliderValue(float newValue)
    {
        currentPercentage = (int)newValue;
        oxygenSlider.value = currentPercentage;
    }
    
    public void SetTimerValue(int seconds)
    {
        percentageText.text = $"{seconds}s";
    }

}
