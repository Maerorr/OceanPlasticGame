using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceCannon : MonoBehaviour
{
    private PlayerInput playerInput;
    private float previousFrameMagnitude = 0f;

    private float shootThreshold = 0.9f;
    [SerializeField]
    private float cooldown = 1f;
    private bool canShoot = true;
    private float cooldownTimer = 0f;
    private Coroutine cooldownCoroutine;
    
    [SerializeField]
    private float cannonRange = 3f;
    [SerializeField]
    private float cannonPower = 5f;
    [SerializeField]
    private float maxConeAngle = 45f;
    
    [SerializeField] 
    private Transform castStart;
    
    [SerializeField] 
    private Vector2 boxCastSize;
    
    [SerializeField]
    LayerMask layerMask;
    
    [SerializeField]
    SpriteRenderer spriteRenderer;

    private Material shockwaveMaterial;
    private float waveTime = 0;
    
    private RaycastHit2D[] hits = new RaycastHit2D[32];
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        shockwaveMaterial = spriteRenderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        float inputMag = playerInput.actions["Aim"].ReadValue<Vector2>().magnitude;
        if (inputMag < 0.1f && previousFrameMagnitude > shootThreshold)
        {
            Shoot();
        }
        previousFrameMagnitude = inputMag;
    }
    
    private void Shoot()
    {
        if (canShoot)
        {
            DOTween.To(() => waveTime, x => waveTime = x, 1f, 0.5f).OnUpdate(() =>
            {
                shockwaveMaterial.SetFloat("_ShootTime", waveTime);
            }).OnComplete(() =>
            {
                waveTime = 0f;
            });
            Array.Clear(hits, 0, hits.Length);
            Rigidbody2D rb;
            int hitCount = Physics2D.BoxCastNonAlloc(castStart.position, boxCastSize, 0f, castStart.right, hits, cannonRange - boxCastSize.x / 2f, layerMask);
            for (int i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                float angle = Vector2.Angle(transform.right, hit.transform.position - castStart.position);
            
                if (angle < maxConeAngle)
                {
                    if (hit.transform.TryGetComponent(out rb))
                    {
                        Vector2 distance = hit.transform.position - castStart.position;
                        rb.AddForce(distance.normalized * cannonPower, ForceMode2D.Impulse);
                    }
                    Debug.DrawLine(castStart.position, hit.transform.position, Color.green);
                }
                else
                {
                    Debug.DrawLine(castStart.position, hit.transform.position, Color.red);
                }
            }
            
            cooldownCoroutine = StartCoroutine(Cooldown());
        }
    }
    
    private IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
