using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrashMeter : MonoBehaviour
{
    [SerializeField]
    private Slider trashSlider;
    [SerializeField]
    private TextMeshProUGUI trashText;
    
    public void SetTrashValue(int currentTrash, int maxTrash)
    {
        trashSlider.maxValue = maxTrash;
        trashSlider.value = currentTrash;
        trashText.text = $"{currentTrash}/{maxTrash}";
    }
}
