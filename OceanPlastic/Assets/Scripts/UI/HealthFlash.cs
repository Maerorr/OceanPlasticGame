using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthFlash : MonoBehaviour
{
    Image image;

    private float defaultAlpha;
    
    Coroutine healthFlashCoroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultAlpha = image.color.a;
        image.color = new Color(1, 1, 1, 0);
    }

    public void Flash()
    {
        if (healthFlashCoroutine != null)
        {
            StopCoroutine(healthFlashCoroutine);
        }
        healthFlashCoroutine = StartCoroutine(FlashCoroutine());
    }
    
    IEnumerator FlashCoroutine()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            image.color = new Color(1, 1, 1, Mathf.Lerp(defaultAlpha, 0, t));
            yield return null;
        }
    }
}
