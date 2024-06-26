using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
    using UnityEditor;
#endif

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
    private float minDistanceFromTerrain = 1f;

    [SerializeField]
    private float width;
    [SerializeField]
    private float height;

    private float softMax;

    private List<GameObject> activeObjects = new List<GameObject>();
    private int initiallySpawnedObjects = 0;

    RaycastHit2D[] raycastHits = new RaycastHit2D[128];

    [SerializeField]
    private EditorGizmoData editorGizmoData;
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

    /*public void ManualSpawn()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            Destroy(activeObjects[i]);
        }
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
    }*/

    private bool SpawnTrash(float randomValue)
    {
        float randomX = UnityEngine.Random.Range(-width / 2f, width / 2f);
        float randomY = UnityEngine.Random.Range(-height / 2f, height / 2f);
        Vector3 pos = transform.position + new Vector3(randomX, randomY, spawnedObjectsZ);
        Vector3 raycastOrigin = transform.position + new Vector3(randomX, randomY, -50f);
        
        Array.Clear(raycastHits, 0, raycastHits.Length);

        int hitCount = Physics2D.CircleCastNonAlloc(raycastOrigin, minDistanceFromAnother / 3f, Vector2.zero, raycastHits);

        for (int i = 0; i < hitCount; i++ )
        {
            
            if (raycastHits[i].transform.TryGetComponent(out Tag tagComponent))
            {
                if (tagComponent.HasTag(Tags.Terrain)) return false;
            }
        }
        
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
                trash.GetComponent<FloatingTrash>().SetSpawnerParent(this);
                activeObjects.Add(trash);
                initiallySpawnedObjects++;
                return true;
            }
        }

        return false;
    }

    public void RemoveTrash(FloatingTrash trash)
    {
        activeObjects.Remove(trash.gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = editorGizmoData.color;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromAnother);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromTerrain);
        Handles.color = editorGizmoData.color;
        Handles.Label(transform.position, editorGizmoData.label);
    }
#endif

    public int GetCurrentObjectsCount()
    {
        return activeObjects.Count;
    }
    
    public int GetMaxTrashCount()
    {
        return initiallySpawnedObjects;
    }

    public int SpawnAdditionalTrash(float percentOfOriginal, int max)
    {
        int spawned = 0;
        int needToSpawn = Mathf.CeilToInt(percentOfOriginal * (float)initiallySpawnedObjects);
        while (spawned < needToSpawn)
        {
            float randomValue = UnityEngine.Random.value;
            if (SpawnTrash(randomValue))
            {
                spawned++;
            }

            if (spawned >= max)
            {
                return spawned;
            }
        }

        return spawned;
    }
}
