using System;
using System.Collections.Generic;
using UnityEngine;

public class Levelinitializer : MonoBehaviour
{
    [Serializable]
    public class TerrainVariations
    {
        public List<GameObject> easy35mTerrainVariants;
        public List<GameObject> medium75mTerrainVariants;
        public List<GameObject> hard150mTerrainVariants;
    }
    
    [SerializeField]
    private TerrainVariations terVars;

    private GameObject instantiatedTerrain;

    private void Awake()
    {
        LevelDifficulty levelDifficulty = StaticLevelData.chosenDifficulty;
        GameObject chosenTerrain = new GameObject();
        switch (levelDifficulty)
        {
            case LevelDifficulty.Easy:
                chosenTerrain = terVars.easy35mTerrainVariants[UnityEngine.Random.Range(0, terVars.easy35mTerrainVariants.Count)];
                
                break;
            case LevelDifficulty.Medium:
                chosenTerrain = terVars.medium75mTerrainVariants[UnityEngine.Random.Range(0, terVars.easy35mTerrainVariants.Count)];
                break;
            case LevelDifficulty.Hard:
                chosenTerrain = terVars.hard150mTerrainVariants[UnityEngine.Random.Range(0, terVars.easy35mTerrainVariants.Count)];
                break;
        }
        instantiatedTerrain = Instantiate(chosenTerrain, Vector3.zero, Quaternion.identity);
    }
}
