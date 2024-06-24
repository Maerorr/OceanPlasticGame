using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainTransition : MonoBehaviour
{
    // treated equally as if it was one sprite. But we have 'islands' which are different objects
    public List<SpriteShapeRenderer> lowerSprites;
    public float transitionStartLevel;
    public float transitionEndLevel;
    private Color initialColor;

    private void Start()
    {
        initialColor = lowerSprites[0].color;
    }

    private void Update()
    {
        float a = (PlayerManager.Instance.Position().y - transitionStartLevel) /
                  (transitionEndLevel - transitionStartLevel);
        initialColor.a = Mathf.Clamp01(a);
        foreach (var sr in lowerSprites)
        {
            sr.color = initialColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(-100f, transitionStartLevel, -5f), new Vector3(100f, transitionStartLevel, -5f));
        Gizmos.DrawLine(new Vector3(-100f, transitionEndLevel, -5f), new Vector3(100f, transitionEndLevel, -5f));
    }
}
