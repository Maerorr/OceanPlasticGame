using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchDetectSquare : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    SpriteRenderer spriteRenderer;
    bool hasBeenTouched = false;

    public UnityEvent onTouched;
    private Collider2D collider2D;

    public Sprite brokenSprite;
    public Sprite fixedSprite;
    
    private Coroutine repairCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = brokenSprite;
        collider2D = GetComponent<Collider2D>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasBeenTouched)
        {
            return;
        }
        
        repairCoroutine = StartCoroutine(Repair());
    }

    IEnumerator Repair()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        spriteRenderer.sprite = fixedSprite;
        hasBeenTouched = true;
        //spriteRenderer.color = Color.red;
        onTouched.Invoke();
        collider2D.enabled = false;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
        }
    }
    
    public bool HasBeenTouched()
    {
        return hasBeenTouched;
    }
}
