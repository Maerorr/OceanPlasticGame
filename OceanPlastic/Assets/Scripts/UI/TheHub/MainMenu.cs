using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private List<MenuEntry> menuEntries;

    private void Start()
    {
        foreach (var menuEntry in menuEntries)
        {
            menuEntry.initialPos = menuEntry.rect.anchoredPosition;
        }
    }

    public void LevelsPress()
    {
        MoveEntry(MenuEntryType.Levels);
    }
    
    public void UpgradesPress()
    {
        MoveEntry(MenuEntryType.Upgrades);
    }
    
    public void SettingsPress()
    {
        MoveEntry(MenuEntryType.Settings);
    }

    private void MoveEntry(MenuEntryType type)
    {
        var entry = menuEntries.Find(x => x.type == type);
        if (entry.isActive)
        {
            entry.isActive = false;
            entry.rect.DOAnchorPos(entry.initialPos, 0.75f).OnComplete(
                () => entry.rect.gameObject.SetActive(false)
            );
        }
        else
        {
            entry.rect.gameObject.SetActive(true);
            entry.isActive = true;
            entry.rect.DOAnchorPos(new Vector2(0f, 0f), 0.75f);
        }
    }
}

[Serializable]
public class MenuEntry
{
    public RectTransform rect;
    public MenuEntryType type;
    public bool isActive;
    public Vector2 initialPos;
}

[Serializable]
public enum MenuEntryType
{
    Levels,
    Upgrades,
    Settings,
}
