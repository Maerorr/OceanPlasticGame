using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private List<MenuEntry> menuEntries;

    public GameObject noise;
    
    private Coroutine switchPanelCoroutine;

    private void Awake()
    {
        noise.SetActive(false);
        foreach (var entry in menuEntries)
        {
            entry.rect.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        foreach (var menuEntry in menuEntries)
        {
            menuEntry.initialPos = menuEntry.rect.anchoredPosition;
        }
    }

    public void LevelsPress()
    {
        // MoveEntry(MenuEntryType.Levels);
        // MoveEntry(MenuEntryType.Upgrades, true);
        // MoveEntry(MenuEntryType.Settings, true);
        
        if (switchPanelCoroutine != null)
        {
            StopCoroutine(switchPanelCoroutine);
        }

        if (menuEntries.Find(x => x.type == MenuEntryType.Levels).isActive)
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.None));
        }
        else
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.Levels));
        }
    }
    
    public void UpgradesPress()
    {
        // MoveEntry(MenuEntryType.Upgrades);
        // MoveEntry(MenuEntryType.Levels, true);
        // MoveEntry(MenuEntryType.Settings, true);
        
        if (switchPanelCoroutine != null)
        {
            StopCoroutine(switchPanelCoroutine);
        }
        //switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.Upgrades));
        if (menuEntries.Find(x => x.type == MenuEntryType.Upgrades).isActive)
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.None));
        }
        else
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.Upgrades));
        }
    }
    
    public void SettingsPress()
    {
        // MoveEntry(MenuEntryType.Settings);
        // MoveEntry(MenuEntryType.Levels, true);
        // MoveEntry(MenuEntryType.Upgrades, true);
        
        if (switchPanelCoroutine != null)
        {
            StopCoroutine(switchPanelCoroutine);
        }
        //switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.Settings));
        if (menuEntries.Find(x => x.type == MenuEntryType.Settings).isActive)
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.None));
        }
        else
        {
            switchPanelCoroutine = StartCoroutine(SwitchPanel(MenuEntryType.Settings));
        }
    }

    IEnumerator SwitchPanel(MenuEntryType type)
    {
        noise.SetActive(true);
        foreach (var entry in menuEntries)
        {
            if (entry.type == type)
            {
                Debug.Log("enabling " + entry.type + " entry");
                entry.rect.gameObject.SetActive(true);
                entry.isActive = true;
            }
            else
            {
                entry.rect.gameObject.SetActive(false);
                entry.isActive = false;
            }
        }
        yield return new WaitForSeconds(0.1f);
        noise.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
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
    None,
    Levels,
    Upgrades,
    Settings,
}
