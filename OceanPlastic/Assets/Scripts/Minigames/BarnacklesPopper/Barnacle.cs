using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Barnacle : MonoBehaviour, IPointerDownHandler
{
    public int tapsRequired = 10;
    private int currentTaps;
    private bool tapped = false;
    public float requiredTimeBetweenTaps = 0.2f;
    private float lastTapTime = 0f;
    
    SpriteRenderer spriteRenderer;
    ParticleSystem particleSystem;
    private Color initialColor;

    public bool isPopped;
    public BarnaclePopper barnaclePopper;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        initialColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (tapped && !isPopped)
        {
            if (Time.time - lastTapTime < requiredTimeBetweenTaps)
            {
                currentTaps++;
                particleSystem.Emit(3);
                Color col = initialColor;
                col.a = 1 - (float)currentTaps / tapsRequired;
                spriteRenderer.color = col;
                if (currentTaps >= tapsRequired)
                {
                    spriteRenderer.enabled = false;
                    tapped = false;
                    isPopped = true;
                    barnaclePopper.PopBarnacle();
                }
            }
            lastTapTime = Time.time;
        }
        tapped = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tapped = true;
    }
    
    
}
