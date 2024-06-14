using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private List<MenuEntry> menuEntries;

    public GameObject noise;
    
    private Coroutine switchPanelCoroutine;

    private void Start()
    {
        Application.targetFrameRate = 60;
        noise.SetActive(false);
        foreach (var entry in menuEntries)
        {
            entry.rect.gameObject.SetActive(false);
            entry.initialPos = entry.rect.anchoredPosition;
        }
    }

    public void LevelsPress()
    {
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
        if (switchPanelCoroutine != null)
        {
            StopCoroutine(switchPanelCoroutine);
        }

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
        if (switchPanelCoroutine != null)
        {
            StopCoroutine(switchPanelCoroutine);
        }

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
