using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBubble : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    
private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        transform.position += 5f * Vector3.up * Time.deltaTime;
        if (transform.position.y > -spriteRenderer.size.y / 2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                PlayerManager.Instance.Player.AddOxygen(2f);
                Destroy(gameObject);
            }
        }
    }
}
