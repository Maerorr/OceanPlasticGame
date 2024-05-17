using System;
using UnityEngine;

public class FloatingTrash : MonoBehaviour
{
    [SerializeField] private FloatingTrashSO data;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        int size = data.spriteVariants.Count;
        var randomSprite = data.spriteVariants[UnityEngine.Random.Range(0, size)];
        spriteRenderer.sprite = randomSprite;
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
    }
    
    public FloatingTrashSO GetData()
    {
        return data;
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
