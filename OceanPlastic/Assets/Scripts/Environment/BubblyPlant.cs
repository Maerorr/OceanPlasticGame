using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblyPlant : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float minTimeBetweenBubbles = 0.75f;
    public float maxTimeBetweenBubbles = 3f;
    
    private void Start()
    {
        StartCoroutine(SpawnBubbles());
    }
    
    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenBubbles, maxTimeBetweenBubbles));
            GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            bubble.transform.SetParent(transform);
            bubble.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0.1f);
        }
    }
}
