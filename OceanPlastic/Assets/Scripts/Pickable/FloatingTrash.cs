using System;
using UnityEngine;

public class FloatingTrash : MonoBehaviour
{
    [SerializeField]
    private FloatingTrashSO data;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory pi = other.GetComponentInParent<PlayerInventory>();
        if (pi != null)
        {
            if (pi.AddItem(data))
            {
                Destroy(gameObject);
            }
        }
    }
}
