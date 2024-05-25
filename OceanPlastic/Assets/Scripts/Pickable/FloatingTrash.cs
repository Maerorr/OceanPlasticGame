using System.Collections;
using UnityEngine;

public class FloatingTrash : MonoBehaviour
{
    [SerializeField] private FloatingTrashSO data;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    Coroutine moveCoroutine;
    private TrashSpawner spawnerParent;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        int size = data.spriteVariants.Count;
        var randomSprite = data.spriteVariants[UnityEngine.Random.Range(0, size)];
        spriteRenderer.sprite = randomSprite;
        transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
    }

    public void SetSpawnerParent(TrashSpawner parent)
    {
        spawnerParent = parent;
    }
    
    public FloatingTrashSO GetData()
    {
        return data;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tag tags = other.gameObject.GetComponent<Tag>();
        if (tags != null)
        {
            if (!tags.HasTag(Tags.Vacuum)) return;
        }
        PlayerInventory pi = other.GetComponentInParent<PlayerInventory>();
        if (pi != null)
        {
            if (pi.AddItem(data))
            {
                spawnerParent.RemoveTrash(this);
                Destroy(gameObject);
            }
        }
    }

    public void MoveTowardsVacuum(Vector2 velocity)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(Move(velocity));
    }
    
    IEnumerator Move(Vector2 velocity)
    {
        while (true)
        {
            rb.velocity = velocity;
            yield return new WaitForSeconds(0.05f);
            velocity = Vector2.Lerp(velocity, Vector2.zero, 0.1f);
            if (velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                break;
            }
        }
    }
}
