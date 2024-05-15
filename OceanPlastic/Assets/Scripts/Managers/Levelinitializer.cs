using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Levelinitializer : MonoBehaviour
{
    [SerializeField]
    private TerrainVariants terrainVariants;
    
    [SerializeField]
    private EnvironmentData environmentData;

    private GameObject instantiatedTerrain;
    private List<GameObject> instantiatedFish = new List<GameObject>();

    private void Awake()
    {
        LevelDifficulty levelDifficulty = StaticLevelData.chosenDifficulty;
        Debug.Log(levelDifficulty);
        SpawnTerrain(levelDifficulty);
        SpawnAllFishSchools(levelDifficulty);
    }

    private void SpawnTerrain(LevelDifficulty difficulty)
    {
        GameObject chosenTerrain;
        switch (difficulty)
        {
            case LevelDifficulty.Easy:
                chosenTerrain = terrainVariants.easy35mTerrainVariants[Random.Range(0, terrainVariants.easy35mTerrainVariants.Count)];
                break;
            case LevelDifficulty.Medium:
                chosenTerrain = terrainVariants.medium75mTerrainVariants[Random.Range(0, terrainVariants.easy35mTerrainVariants.Count)];
                break;
            case LevelDifficulty.Hard:
                chosenTerrain = terrainVariants.hard150mTerrainVariants[Random.Range(0, terrainVariants.easy35mTerrainVariants.Count)];
                break;
            default:
                chosenTerrain = new GameObject("NO MATCHING TERRAIN");
                break;
        }
        instantiatedTerrain = Instantiate(chosenTerrain, Vector3.zero, Quaternion.identity);
        instantiatedTerrain.name = "Terrain";
    }
    
    private void SpawnAllFishSchools(LevelDifficulty difficulty)
    {
        int maxDepth = (int)difficulty;
        int fishSchoolCount = (int)(maxDepth * environmentData.fishDensity);
        int iterJump = maxDepth / fishSchoolCount;
        for (int i = 0; i < maxDepth; i+= iterJump)
        {
            // get random depth
            float depth = 4 + (float)i + Random.Range(-iterJump/2f, iterJump/2f);
            if (depth > 0 && depth <= 35)
            {
                SpawnFish(
                    environmentData.fish35mVariants[Random.Range(0, environmentData.fish35mVariants.Count)],
                    depth);
            }
            else if (depth > 35 && depth <= 75)
            {
                SpawnFish(
                    environmentData.fish75mVariants[Random.Range(0, environmentData.fish75mVariants.Count)],
                    depth);
            }
            else if (depth > 75 && depth <= 150)
            {
                SpawnFish(
                    environmentData.fish150mVariants[Random.Range(0, environmentData.fish150mVariants.Count)],
                    depth);
            }
            else
            {
                Debug.Log("Depth out of range");
            }
        }
    }

    private void SpawnFish(GameObject fish, float depth)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-20, 20), -depth, 11f);
        GameObject fishSchoolObject = Instantiate(fish, spawnPosition, Quaternion.identity);
        fishSchoolObject.name = "FishSchool";
        FishSchool fishSchool = fishSchoolObject.GetComponent<FishSchool>();
        float speed = Random.Range(-1.5f, 1.5f);
        // make the value not close to zero
        if (speed < 0.5f && speed > -0.5f)
        {
            speed = 0.5f;
        }
        fishSchool.SetData(Random.Range(3f, 10f), Random.Range(5, 15), speed);
        instantiatedFish.Add(fishSchool.gameObject);
    }
}

[Serializable]
public class TerrainVariants
{
    public List<GameObject> easy35mTerrainVariants;
    public List<GameObject> medium75mTerrainVariants;
    public List<GameObject> hard150mTerrainVariants;
}

[Serializable]
public class EnvironmentData
{
    public List<GameObject> fish35mVariants;
    public List<GameObject> fish75mVariants;
    public List<GameObject> fish150mVariants;
    public float fishDensity = 0.2f;
}