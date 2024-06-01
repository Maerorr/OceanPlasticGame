using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveEntry : MonoBehaviour
{
    [SerializeField]
    private string objectiveText;
    [SerializeField]
    private TextMeshProUGUI progressText;
    public Image spriteImage;
    public FloatingTrashSO trash;
    public ObjectiveEntryType type;
    
    public string trashName;

    public int maxValue;
    public int currentValue;
    
    public void SetSprite(Sprite sprite)
    {
        spriteImage.sprite = sprite;
        UpdateText();
    }
    
    public void SetEntryType(ObjectiveEntryType type)
    {
        this.type = type;
    }

    public void SetObjectiveText(string text)
    {
        objectiveText = text;
        UpdateText();
    }
    
    public void SetMaxValue(int value)
    {
        maxValue = value;
        UpdateText();
    }

    public void AddProgress(int progress)
    {
        currentValue += progress;
        UpdateText();
    }

    private void UpdateText()
    {
        progressText.text = $"{objectiveText} {currentValue}/{maxValue}";
    }
    
    public void SetTrashName(string name)
    {
        trashName = name;
    }
    
    public string GetTrashName()
    {
        return trashName;
    }
    
    public ObjectiveEntryType GetEntryType()
    {
        return type;
    }
}

public enum ObjectiveEntryType
{
    Trash,
    Pipe
}
