using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyJellyfish : MonoBehaviour
{
    public SpriteRenderer sprite;

    public float yMoveRange = 1f;
    public float loopTime = 3f;

    private float initialY;
    
    private void Start()
    {
        initialY = transform.position.y;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(initialY - yMoveRange, loopTime).SetEase(Ease.InOutQuad)); // move down
        seq.Append(transform.DOMoveY(initialY , loopTime).SetEase(Ease.InOutQuad)); // move up
        seq.SetLoops(-1);
        seq.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                PlayerManager.Instance.Player.TakeDamage(10f);
            }
        }
    }
}
