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
    
    [SerializeField]
    private float shootThreshold = 0.75f;
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

    private int aimOverlayID = Shader.PropertyToID("_IsAiming");
    private int shootTweenID;
    
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
        if (inputMag < 0.1f)
        {
            DisableAimOverlay();
            if (previousFrameMagnitude > shootThreshold)
            {
                Shoot();
            }
        }
        else
        {
            EnableAimOverlay();
        }
        previousFrameMagnitude = inputMag;
    }

    private void EnableAimOverlay()
    {
        shockwaveMaterial.SetInt(aimOverlayID, 1);
    }
    
    private void DisableAimOverlay() 
    {
        shockwaveMaterial.SetInt(aimOverlayID, 0);
    }
    
    private void Shoot()
    {
        if (canShoot)
        {
            DOTween.Kill(shootTweenID);
            shootTweenID = DOTween.To(() => waveTime, x => waveTime = x, 1f, 0.5f).OnUpdate(() =>
            {
                shockwaveMaterial.SetFloat("_ShootTime", waveTime);
            }).OnComplete(() =>
            {
                waveTime = 0f;
            }).SetId(999).intId;
            
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
                        EnemyFish fish;
                        if (hit.transform.TryGetComponent(out fish))
                        {
                            fish.TriggerIgnorePlayer();
                        }
                        Vector2 direction = hit.transform.position - castStart.position;
                        rb.AddForce(direction.normalized * cannonPower, ForceMode2D.Impulse);
                    }
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
