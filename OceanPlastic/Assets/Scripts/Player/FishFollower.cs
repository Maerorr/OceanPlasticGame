using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFollower : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 target;
    [SerializeField]
    private float speed = 5f;

    private SpriteRenderer sr;

    private Vector2 velocity;

    [SerializeField] private float targetOffset = 1f;
    
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = (player.position - transform.position);
        Vector2 dirNormalized = dir.normalized;
        target = (Vector2)player.position - dirNormalized * targetOffset;
        DrawCircle(target, 0.5f);
        var newVelocity = dirNormalized;
        velocity = Vector2.Lerp(velocity, newVelocity, Time.deltaTime * 4f) * speed;
        //velocity *= Mathf.Clamp01(Vector2.Distance(transform.position, target));

        velocity *= Mathf.Clamp01(Vector2.Distance(transform.position, player.position) - targetOffset);
        
        sr.flipY = velocity.x < 0;
        rb.velocity = velocity;
        velocity = velocity.normalized;
        rb.MoveRotation(Quaternion.Euler(0f, 0f, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg));
    }
    
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
