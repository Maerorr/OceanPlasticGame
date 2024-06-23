using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TutorialFloatingTrash : MonoBehaviour
{
    [SerializeField] private FloatingTrashSO data;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    Coroutine moveCoroutine;
    private Tutorial tut;
    public UnityEvent onCollected;
    
    private void Awake()
    {
        tut = FindAnyObjectByType<Tutorial>();
        rb = GetComponent<Rigidbody2D>();
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
        Tag tags = other.gameObject.GetComponent<Tag>();
        if (tags != null)
        {
            if (!tags.HasTag(Tags.Vacuum)) return;
        }
        PlayerInventory pi = other.GetComponentInParent<PlayerInventory>();
        if (pi != null)
        {
            tut.EnableNextStep();
            onCollected.Invoke();
            pi.AddItem(data);
            Destroy(gameObject);
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
