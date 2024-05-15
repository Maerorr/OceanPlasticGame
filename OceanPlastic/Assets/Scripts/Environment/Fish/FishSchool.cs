using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishData
{
    public Transform fish;
    public float phase;
    public float frequency;
    public float yAmplitude;
}

public class FishSchool : MonoBehaviour
{
    private List<FishData> fish = new List<FishData>();
    
    [SerializeField]
    float fishXSpeed = 1f;

    [SerializeField]
    float fishYAmplitude = 0.1f;
    
    [SerializeField]
    Sprite fishSprite;
    
    [SerializeField]
    int fishCount = 10;
    
    [SerializeField]
    float fishMinSize = 0.5f;
    
    [SerializeField]
    float fishMaxSize = 1.5f;
    
    [SerializeField]
    float schoolSize = 3f;

    [SerializeField] 
    private float boundXleft;

    [SerializeField] 
    private float boundXright;

    public void SetData(float ySize, int fishCount, float fishXspeed)
    {
        fishXSpeed = fishXspeed;
        schoolSize = ySize;
        this.fishCount = fishCount;
    }
    
    private void Awake()
    {
        var z = transform.position.z;
        for (int i = 0; i < fishCount; i++)
        {
            var fishObj = new GameObject("Fish");
            fishObj.transform.parent = transform;
            fishObj.transform.localPosition = new Vector3(Random.Range(-schoolSize, schoolSize), Random.Range(-schoolSize, schoolSize), z);
            // check if it's not too close to other fish
            for (int j = 0; j < fish.Count; j++)
            {
                if (Vector3.Distance(fishObj.transform.position, fish[j].fish.position) < 0.5f)
                {
                    Destroy(fishObj);
                    i--;
                    goto SkipFish;
                }
            }
            float scale = Random.Range(fishMinSize, fishMaxSize);
            fishObj.transform.localScale = new Vector3(scale, scale, 1f);
            var fishSpriteRenderer = fishObj.AddComponent<SpriteRenderer>();
            fishSpriteRenderer.sprite = fishSprite;
            fishSpriteRenderer.sortingOrder = 0;
            fishSpriteRenderer.color = Color.white;
            var fishData = new FishData
            {
                fish = fishObj.transform,
                phase = Random.Range(0f, 2f * Mathf.PI),
                frequency = Random.Range(1f, 3f),
                yAmplitude = fishYAmplitude
            };
            fish.Add(fishData);
            
            SkipFish:
            { }
        }
    }

    private void Update()
    {
        for (int i = 0; i < fish.Count; i++)
        {
            FishData fishData = fish[i];
            Vector3 oldPos = fishData.fish.position;
            Vector3 pos = oldPos;
            pos.x -= fishXSpeed * Time.deltaTime;
            var sin1 = Mathf.Sin(Time.time * fishData.frequency + fishData.phase);
            var sin2 = Mathf.Sin(Time.time * fishData.frequency * 1.4f + fishData.phase);
            var sin3 = Mathf.Sin(Time.time * fishData.frequency * 2.2f + fishData.phase);
            
            pos.y += (sin1 + sin2 / 2.2f + sin3 / 3.3f) * fishData.yAmplitude * Time.deltaTime;
            
            fishData.fish.position = pos;
            float deltaX = oldPos.x - pos.x;
            float deltaY = oldPos.y - pos.y;
            fishData.fish.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg);
            if (fishXSpeed < 0f)
            {
                // flip sprite horizontally if moving left
                fishData.fish.localScale = new Vector3(-1, 1, 1);
            }
            
            // check if fish is out of bounds
            if (pos.x < boundXleft)
            {
                pos.x = boundXright;
                fishData.fish.position = pos;
            }
            else if (pos.x > boundXright)
            {
                pos.x = boundXleft;
                fishData.fish.position = pos;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var centerX = (boundXleft + boundXright) / 2;
        var scale = Mathf.Abs(boundXleft) + Mathf.Abs(boundXright);
        Gizmos.DrawWireCube(new Vector3(centerX, transform.position.y, transform.position.z), new Vector3(scale, schoolSize * 2, 1));
    }
}
