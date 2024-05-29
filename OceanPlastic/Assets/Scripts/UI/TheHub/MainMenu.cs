using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private List<MenuEntry> menuEntries;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

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
        MoveEntry(MenuEntryType.Upgrades, true);
        MoveEntry(MenuEntryType.Settings, true);
    }
    
    public void UpgradesPress()
    {
        MoveEntry(MenuEntryType.Upgrades);
        MoveEntry(MenuEntryType.Levels, true);
        MoveEntry(MenuEntryType.Settings, true);
    }
    
    public void SettingsPress()
    {
        MoveEntry(MenuEntryType.Settings);
        MoveEntry(MenuEntryType.Levels, true);
        MoveEntry(MenuEntryType.Upgrades, true);
    }

    private void MoveEntry(MenuEntryType type, bool forceDisable = false)
    {
        var entry = menuEntries.Find(x => x.type == type);
        if (entry.pressCooldown) return;
        if (forceDisable)
        {
            if (entry.isActive)
            {
                entry.isActive = false;
                entry.rect.DOAnchorPos(entry.initialPos, 0.75f).OnComplete(
                    () => entry.rect.gameObject.SetActive(false)
                );
            }
            StartCoroutine(PressCooldown(entry));
            return;
        }
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
        StartCoroutine(PressCooldown(entry));
    }
    
    IEnumerator PressCooldown(MenuEntry entry)
    {
        entry.pressCooldown = true;
        yield return new WaitForSeconds(0.76f);
        entry.pressCooldown = false;
    }
}

[Serializable]
public class MenuEntry
{
    public RectTransform rect;
    public MenuEntryType type;
    public bool isActive;
    public Vector2 initialPos;
    public bool pressCooldown;
}

[Serializable]
public enum MenuEntryType
{
    Levels,
    Upgrades,
    Settings,
}
