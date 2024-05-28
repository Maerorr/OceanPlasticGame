using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private LevelData easyScene;
    [SerializeField] private LevelData mediumScene;
    [SerializeField] private LevelData hardScene;

    public void SelectEasyLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Easy);
        StartGame(easyScene);
    }
    
    public void SelectMediumLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Medium);
        StartGame(mediumScene);
    }
    
    public void SelectHardLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Hard);
        StartGame(hardScene);
    }

    private void StartGame(LevelData level)
    {
        SceneManager.LoadScene("Scenes/Levels/" + level.LevelPath);
        foreach (var obj in level.objectives)
        {
            if (obj.type == ObjectiveType.Trash)
                StaticLevelData.AddCollectionObjective(obj.trash);
        }
    }
}
