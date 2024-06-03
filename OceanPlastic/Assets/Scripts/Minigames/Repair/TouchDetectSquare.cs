using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchDetectSquare : MonoBehaviour, IPointerEnterHandler
{
    SpriteRenderer spriteRenderer;
    bool hasBeenTouched = false;

    public UnityEvent onTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasBeenTouched)
        {
            return;
        }
        hasBeenTouched = true;
        spriteRenderer.color = Color.red;
        onTouched.Invoke();
    }
    
    public bool HasBeenTouched()
    {
        return hasBeenTouched;
    }
}
