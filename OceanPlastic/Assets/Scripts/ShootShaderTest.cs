using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class ShootShaderTest : MonoBehaviour
{
    
    MaterialPropertyBlock propertyBlock;
    Material material;
    SpriteRenderer spriteRenderer;
    private float time = 0f;
    Coroutine shootCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.sharedMaterial;
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (shootCoroutine is null)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        DOTween.To(() => time, x => time = x, 1f, 0.5f).OnUpdate(() =>
        {
            material.SetFloat("_ShootTime", time);
        });
        yield return new WaitForSeconds(1f);
        shootCoroutine = null;
        time = 0f;
    }
}
