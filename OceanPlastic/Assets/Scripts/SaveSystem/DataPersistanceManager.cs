using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManagere : MonoBehaviour
{
    public static DataPersistanceManagere Instance { get; private set; }
    public GameData gameData;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        SaveGame();
    }
    
    public void LoadGame()
    {
        if (this.gameData is null)
        {
            NewGame();
        }
    }
    
    public void SaveGame()
    {
        //SaveSystem.SaveData(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
