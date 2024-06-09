using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private float initialY;
    private bool canFloat = false;
    private float time = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Tag tag))
        {
            if (tag.HasTag(Tags.Player))
            {
                PlayerManager.Instance.PlayerInventory.SetHasKey(true);
                FindAnyObjectByType<GameUIController>().UpdateKey(true);
                Destroy(gameObject);
            }
        }
    }
    
    public void StartFloating()
    {
        initialY = transform.position.y;
        canFloat = true;
    }

    private void Update()
    {
        if (!canFloat) return;
        time += 2f * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, initialY + Mathf.Sin(time) * 0.2f, transform.position.z);
    }
}
