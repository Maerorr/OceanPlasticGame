using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField]
    float baseValue = 0.5f;
    [SerializeField]
    float maxOffset = 0.1f;
    [SerializeField]
    bool hasShaderGraph = false;
    
    void Start()
    {
        if (hasShaderGraph)
        {
            SetShaderGraphColor();
        }
        else
        {
            SetSpriteRendererColor();
        }
    }

    Color GetRandomColor()
    {
        return new Color(
            baseValue + Random.Range(-maxOffset, maxOffset),
            baseValue + Random.Range(-maxOffset, maxOffset),
            baseValue + Random.Range(-maxOffset, maxOffset)
        );
    }
    
    void SetSpriteRendererColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GetRandomColor();
    }
    
    void SetShaderGraphColor()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        GetComponent<SpriteRenderer>().GetPropertyBlock(block);
        block.SetColor("_Color", GetRandomColor());   
        GetComponent<SpriteRenderer>().SetPropertyBlock(block);
    }
}
