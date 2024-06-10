using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblyPlant : MonoBehaviour
{
    public GameObject bubblePrefab;
    
    private void Start()
    {
        StartCoroutine(SpawnBubbles());
    }
    
    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.75f, 3f));
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            bubble.transform.SetParent(transform);
            bubble.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0.1f);
        }
    }
}
