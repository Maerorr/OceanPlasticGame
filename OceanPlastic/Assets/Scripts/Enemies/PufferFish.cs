using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PufferFish : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite puffedSprite;
    
    [SerializeField] private Vector2 patrolArea;
    [SerializeField] private float speed;
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private bool reachedTarget;
    private Vector2 target;
    private Vector2 noiseMove;
    private Vector2 direction;
    
    private SpriteRenderer spriteRenderer;
    private float currentSpeed;
    
    Coroutine puffCheckCoroutine;
    Coroutine puffCoroutine;
    private bool puffed = false;
    private bool recentlyPuffed = false;
    private float unpuffCooldown = 2f;
    private float unpuffTimer = 0f;
    
    public ParticleSystem bubbles;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        target = initialPosition + new Vector3(-patrolArea.x, 0, 0);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        puffCheckCoroutine = StartCoroutine(PuffCheck());
        spriteRenderer.sprite = normalSprite;
        bubbles = GetComponentInChildren<ParticleSystem>();
        bubbles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }
    
    private void Patrol()
    {
        if (reachedTarget)
        {
            if (transform.position.x > initialPosition.x)
            {
                float targetX = Random.Range(initialPosition.x - patrolArea.x, initialPosition.x - patrolArea.x / 2f);
                float targetY = Random.Range(initialPosition.y - patrolArea.y, initialPosition.y);
                target = new Vector2(targetX, targetY);
            }
            else
            {
                float targetX = Random.Range(initialPosition.x + patrolArea.x / 2f, initialPosition.x + patrolArea.x);
                float targetY = Random.Range(initialPosition.y, initialPosition.y + patrolArea.y);
                target = new Vector2(targetX, targetY);
            }
        }

        if (Vector2.Distance(transform.position, target) < 1f)
        {
            reachedTarget = true;
        }
        else
        {
            reachedTarget = false;
        }
        direction = (target - (Vector2)transform.position).normalized;
        
        spriteRenderer.flipY = direction.x < 0;
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * direction;
        rb.MoveRotation(Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
    }

    IEnumerator PuffCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var dist = Vector2.Distance(PlayerManager.Instance.Position(), transform.position);
        
            if (dist < 2.3f)
            {
                if (!puffed)
                {
                    puffed = true;
                    if (puffCoroutine != null)
                    {
                        StopCoroutine(puffCoroutine);
                    }

                    puffCoroutine = StartCoroutine(Puff(true));
                    unpuffTimer = 0f;
                }
            }
            else
            {
                if (unpuffTimer >= unpuffCooldown && puffed)
                {
                    if (puffCoroutine != null)
                    {
                        StopCoroutine(puffCoroutine);
                    }
                    puffCoroutine = StartCoroutine(Puff(false));
                    unpuffTimer = 0f;
                }
                unpuffTimer += 0.1f;
            }
            
        }
    }

    IEnumerator Puff(bool puff)
    {
        if (puff)
        {
            transform.DOScaleY(0.33f, 0.1f).OnComplete(
                () => { transform.DOScaleY(0.5f, 0.1f); });
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.sprite = puffedSprite;
            puffed = true;
        }
        else
        {
            transform.DOScaleY(0.75f, 0.25f);
            bubbles.Play();
            yield return new WaitForSeconds(0.25f);
            transform.DOScaleY(0.5f, 0f);
            spriteRenderer.sprite = normalSprite;
            puffed = false;
            yield return new WaitForSeconds(0.1f);
            bubbles.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                if (puffed)
                {
                    PlayerManager.Instance.Player.TakeDamage(5f);
                }
            }
        }
    
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, patrolArea * 2);
    }
#endif
    
    private void DrawCircle(Vector2 center, float radius)
    {
        float angle = 0f;
        float angleIncrease = (2f * Mathf.PI) / 16;
        Vector2 lastPos = Vector2.zero;
        for (int i = 0; i < 16 + 1; i++)
        {
            Vector2 newPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius + center;
            if (i > 0)
            {
                Debug.DrawLine(lastPos, newPos);
            }
            lastPos = newPos;
            angle += angleIncrease;
        }
    }
}
