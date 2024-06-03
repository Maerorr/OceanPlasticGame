using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFish : MonoBehaviour
{
    private EnemyState state;
    [SerializeField] private Vector2 patrolArea;
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeedup;
    [SerializeField] private float range;
    private Vector3 initialPosition;
    private Rigidbody2D rb;
    private bool reachedTarget;
    private Vector2 target;
    private Vector2 noiseMove;
    private Vector2 direction;
    
    private bool canAttack = true;
    [SerializeField] private float attackCooldown;
    
    private SpriteRenderer spriteRenderer;
    private float currentSpeed;
    private bool isNearPlayer;
    private bool ignoresPlayer = false;
    private Coroutine stateCheckCoroutine;
    private Coroutine ignorePlayerCooldownCoroutine;
    private Coroutine attackCooldownCoroutine;
    
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        target = initialPosition + new Vector3(-patrolArea.x, 0, 0);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        stateCheckCoroutine = StartCoroutine(StateCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Patrol)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
    }

    private void EnterChase()
    {
        state = EnemyState.Chase;
        spriteRenderer.color = new Color(1f, 0.8f, 0.8f);
    }
    private void EnterPatrol()
    {
        state = EnemyState.Patrol;
        reachedTarget = true;
        spriteRenderer.color = Color.white;
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

    private void Chase()
    {
        target = Vector2.Lerp(target, PlayerManager.Instance.Position(), Time.deltaTime * 4f);
        DrawCircle(target, 0.5f);
        direction = (target - (Vector2)transform.position).normalized;
        //rb.velocity = speed * chaseSpeedup * (direction);
        //rb.MovePosition((Vector2)transform.position + (Time.deltaTime * speed * chaseSpeedup * (direction)));

        if (Vector2.Distance((Vector2)transform.position, PlayerManager.Instance.Position()) < 0.5f)
            isNearPlayer = true;
        
        Debug.DrawRay(transform.position, direction);
        spriteRenderer.flipY = direction.x < 0;
        
        if (!canAttack) return;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.4f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out Tag tag))
            {
                if (tag.HasTag(Tags.Player))
                {
                    if (attackCooldownCoroutine is not null)
                    {
                        StopCoroutine(attackCooldownCoroutine);
                    }
                    attackCooldownCoroutine = StartCoroutine(Attack());
                }
            }
        }
        
    }

    private void FixedUpdate()
    {
        float calcSpeed = state == EnemyState.Patrol ? this.speed : this.speed * chaseSpeedup;
        if (isNearPlayer) calcSpeed /= 4f;
        currentSpeed = Mathf.Lerp(currentSpeed, calcSpeed, Time.deltaTime);
        var pos = new Vector2(transform.position.x, transform.position.y);
        //rb.MovePosition(pos + (Time.deltaTime * calcSpeed * (direction)));
        rb.AddForce(calcSpeed * (direction), ForceMode2D.Force);
        rb.MoveRotation(Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
    }

    IEnumerator StateCheck()
    {
        while (true)
        {
            if (Vector2.Distance(transform.position, PlayerManager.Instance.Position()) < range
                && state != EnemyState.Chase
                && ignoresPlayer == false)
            {
                EnterChase();
            }
            if (Vector2.Distance(transform.position, PlayerManager.Instance.Position()) > range
                && state != EnemyState.Patrol)
            {
                EnterPatrol();
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.05f);
        PlayerManager.Instance.Player.TakeDamage(5f);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TriggerIgnorePlayer()
    {
        StopCoroutine(stateCheckCoroutine);
        if (ignorePlayerCooldownCoroutine is not null)
        {
            StopCoroutine(ignorePlayerCooldownCoroutine);
        }
        ignorePlayerCooldownCoroutine = StartCoroutine(IgnorePlayerCooldown());
    }
    IEnumerator IgnorePlayerCooldown()
    {
        EnterPatrol();
        ignoresPlayer = true;
        yield return new WaitForSeconds(5f);
        ignoresPlayer = false;
        stateCheckCoroutine = StartCoroutine(StateCheck());
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, patrolArea * 2);
        Gizmos.DrawWireSphere(transform.position, range);
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

public enum EnemyState
{
    Patrol,
    Chase,
}
