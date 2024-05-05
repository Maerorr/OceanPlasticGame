using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TrashEntry
{
    [SerializeField]
    public GameObject trashPrefab;
    [SerializeField, Range(0f, 1f)]
    public float spawnChance;
}

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<TrashEntry> trashPrefabs;
    [SerializeField]
    private float spawnedObjectsZ = -1f;
    [SerializeField]
    private float density = 0.1f;
    [SerializeField]
    private float minDistanceFromAnother = 3f;

    [SerializeField]
    private float width;
    [SerializeField]
    private float height;

    private float softMax;

    private List<GameObject> activeObjects = new List<GameObject>();

    private void Awake()
    {
        // normalization of spawn chances
        float sumOfChances = trashPrefabs.Sum(entry => entry.spawnChance);
        foreach (var entry in trashPrefabs)
        {
            entry.spawnChance /= sumOfChances;
        }

        int maxTries = 50;
        int tries = 0;
        
        softMax = width * height * density;
        while (activeObjects.Count < softMax && tries < maxTries)
        {
            float randomValue = UnityEngine.Random.value;
            SpawnTrash(randomValue);
            tries++;
        }
    }

    private bool SpawnTrash(float randomValue)
    {
        float randomX = UnityEngine.Random.Range(-width / 2f, width / 2f);
        float randomY = UnityEngine.Random.Range(-height / 2f, height / 2f);
        Vector3 pos = new Vector3(randomX, randomY, spawnedObjectsZ);
        
        if (activeObjects.Any(obj => Vector3.Distance(obj.transform.position, pos) < minDistanceFromAnother))
        {
            return false;
        }
        
        float sum = 0f;
        foreach (var entry in trashPrefabs)
        {
            sum += entry.spawnChance;
            if (randomValue < sum)
            {
                GameObject trash = Instantiate(entry.trashPrefab, pos, Quaternion.identity);
                activeObjects.Add(trash);
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
    }
}
