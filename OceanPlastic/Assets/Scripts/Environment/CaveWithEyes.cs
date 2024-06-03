using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveWithEyes : MonoBehaviour
{
    [SerializeField]
    private Sprite noEyesSprite;
    [SerializeField]
    private Sprite eyesSprite;
    SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }
    
    private IEnumerator Blink()
    {
        while (true)
        {
            if (Vector3.Distance(PlayerManager.Instance.Position(), transform.position) < 7f)
            {
                spriteRenderer.sprite = noEyesSprite;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                spriteRenderer.sprite = noEyesSprite;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = eyesSprite; 
            }
        }
    }
}
