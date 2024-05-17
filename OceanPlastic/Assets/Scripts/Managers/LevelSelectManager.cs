using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private List<SceneData> easyScenes;
    [SerializeField] private List<SceneData> mediumScenes;
    [SerializeField] private List<SceneData> hardScenes;

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

public class SceneData
{
    public Scene scene;
    public List<FloatingTrashSO> relevantTrash;
}
