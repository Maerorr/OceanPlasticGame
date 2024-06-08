using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleBlock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private List<Transform> childBlocks = new List<Transform>();
    private bool isDragging = false;
    
    private RaycastHit2D[] hits = new RaycastHit2D[8];
    
    BlocksPuzzleController blocksPuzzleController;
    
    private void Start()
    {
        blocksPuzzleController = GetComponentInParent<BlocksPuzzleController>();
        foreach (Transform child in transform)
        {
            childBlocks.Add(child);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        foreach (var tr in childBlocks)
        {
            Array.Clear(this.hits, 0, this.hits.Length);
            int hits = Physics2D.BoxCastNonAlloc(tr.position, new Vector2(0.1f, 0.1f), 0, Vector2.zero, this.hits);
            for (int i = 0; i < hits; i++)
            {
                var hit = this.hits[i];
                if (hit.collider.TryGetComponent(out Tag tag))
                {
                    if (tag.HasTag(Tags.BlockPuzzle))
                    {
                        tag.transform.GetComponent<GridCell>().isChecked = false;
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        foreach (var tr in childBlocks)
        {
            Array.Clear(this.hits, 0, this.hits.Length);
            int hits = Physics2D.BoxCastNonAlloc(tr.position, new Vector2(0.1f, 0.1f), 0, Vector2.zero, this.hits);
            for (int i = 0; i < hits; i++)
            {
                var hit = this.hits[i];
                if (hit.collider.TryGetComponent(out Tag tag))
                {
                    if (tag.HasTag(Tags.BlockPuzzle))
                    {
                        tag.transform.GetComponent<GridCell>().isChecked = true;
                    }
                }
            }
        }
        blocksPuzzleController.Check();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }
}
