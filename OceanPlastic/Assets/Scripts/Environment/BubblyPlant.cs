using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblyPlant : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite beforeBubbleSprite;

    public Transform bubbleSpawnPoint;
    
    public GameObject bubblePrefab;
    public float minTimeBetweenBubbles = 0.75f;
    public float maxTimeBetweenBubbles = 3f;
    
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        StartCoroutine(SpawnBubbles());
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenBubbles - 0.2f, maxTimeBetweenBubbles - 0.2f));
            spriteRenderer.sprite = beforeBubbleSprite;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = normalSprite;
            Vector3 bubbleSpawnPos = bubbleSpawnPoint.position + new Vector3(Random.Range(-0.2f, 0.2f), 0f, 0.1f);
            GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
            bubble.name = "B U B B L E";
            float randSize = Random.Range(0.75f, 1.2f);
            bubble.transform.localScale = new Vector2(randSize, randSize);
        }
    }
}
