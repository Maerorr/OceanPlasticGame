using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public void SelectEasyLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Easy);
        StartGame();
    }
    
    public void SelectMediumLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Medium);
        StartGame();
    }
    
    public void SelectHardLevel()
    {
        StaticLevelData.SetDifficulty(LevelDifficulty.Hard);
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("TerrainSpawnerTest");
    }
}
