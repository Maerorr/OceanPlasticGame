using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private bool isPlayerInside = false;
    private float halfYSize;
    private Player player;
    [SerializeField] 
    private float breathableOffset = 0.5f;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        halfYSize = boxCollider2D.size.y / 2f;
    }

    private void Update()
    {
        Debug.DrawLine(new Vector3(-100f, transform.position.y - halfYSize, -1f), new Vector3(100f, transform.position.y - halfYSize, -1f), Color.red);
        if (!isPlayerInside)
        { 
            return;
        }
        
        if (Mathf.Abs(player.transform.position.y - transform.position.y) < (halfYSize - breathableOffset))
        {
            player.SetIsInWater(false);
        }
        else
        {
            player.SetIsInWater(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;
        
        player.SetCanBreathe(true);
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        player = other.gameObject.GetComponentInParent<Player>();
        if (player == null) return;
        
        player.SetCanBreathe(false);
        isPlayerInside = false;
    }

    private void OnDrawGizmos()
    {
        var box2d = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        pos.y += breathableOffset / 2f;
        Gizmos.DrawWireCube(pos, new Vector3(box2d.size.x, box2d.size.y - breathableOffset, 1f));
    }
}
