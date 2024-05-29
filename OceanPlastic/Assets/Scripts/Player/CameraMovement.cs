using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMoveScript;
    [SerializeField, Range(0f, 1f)] private float smoothTime = 0.5f;
    [SerializeField, Range(0f, 20f)] private float cameraOffset = 5f;
    private float camZ;
    private Vector2 velocity;
    private Vector2 currentOffset;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        camZ = transform.position.z;
    }

    private void Start()
    {
        playerMoveScript ??= FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        Vector2 selfPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPos = playerMoveScript.GetPosition();
        MovementState playerState = playerMoveScript.GetMovementState();
        switch (playerState)
        {
            case MovementState.Idle:
            {
                targetPos = new Vector2(targetPos.x, targetPos.y);
                break;
            }
            case MovementState.Moving:
            {
                var offset = playerMoveScript.GetVelocity().normalized * cameraOffset;
                currentOffset = Vector2.Lerp(currentOffset, offset, 0.6f);
                targetPos = new Vector2(targetPos.x, targetPos.y) + currentOffset;
                break;
            }
        }
        Vector2 newPos = Vector2.SmoothDamp(selfPos, targetPos, ref velocity, smoothTime);
        transform.position = new Vector3(newPos.x, newPos.y, camZ);
    }
}
