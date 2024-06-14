using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = System.Object;

public class PuzzleBlock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private List<Transform> childBlocks = new List<Transform>();
    private bool isDragging = false;
    
    private RaycastHit2D[] hits = new RaycastHit2D[8];
    
    BlocksPuzzleController blocksPuzzleController;
    private Vector3 dragPos;
    
    private void Start()
    {
        blocksPuzzleController = GetComponentInParent<BlocksPuzzleController>();
        foreach (Transform child in transform)
        {
            if (child.name == "sprite") continue;
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
                        var cell = tag.transform.GetComponent<GridCell>();
                        if (cell.isChecked)
                        {
                            if (ReferenceEquals(gameObject, cell.checkedBy))
                            {
                                cell.isChecked = false;
                                cell.checkedBy = null;
                                return;
                            }
                        }
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
                        var cell = tag.transform.GetComponent<GridCell>();
                        cell.isChecked = true;
                        cell.checkedBy = gameObject;
                    }
                }
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        blocksPuzzleController.Check();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.dragging) transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        if (!isDragging) return;
        dragPos = Camera.main.ScreenToWorldPoint(eventData.position);//eventData.pointerCurrentRaycast.worldPosition;
        dragPos.z = -1f;
        transform.position = dragPos;
    }
}
