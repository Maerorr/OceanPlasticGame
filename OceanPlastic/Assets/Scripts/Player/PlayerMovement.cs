using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    Idle,
    Moving,
}

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 velocity;
    private Rigidbody2D rb;
    
    [SerializeField] 
    private float speed = 5f;
    [SerializeField, Range(0f, 0.3f)] 
    private float moveLerpSpeed = 0.1f;
    [SerializeField, Range(0f, 0.3f)] 
    private float rotationLerpSpeed = 0.1f;
    [SerializeField] 
    private ContactFilter2D contactFilter;
    
    private List<RaycastHit2D> hitBuffer = new List<RaycastHit2D>();
    private Vector2 lastMoveDirection = Vector2.zero;
    private MovementState movementState = MovementState.Idle;
    private Player player;
    
    [SerializeField]
    private DepthMeter depthMeter;

    [SerializeField] 
    private Transform waterLevel;
    
    private Coroutine updateDepth;
    
    // Start is called before the first frame update
    void Awake()
    {
        updateDepth = StartCoroutine(UpdateDepth());
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 inputClamped = Vector2.ClampMagnitude(input, 1f);
        CalculateMovementState(inputClamped.magnitude, inputClamped);

        Vector2 newVelocity = CalculateNewVelocity(inputClamped);
        lastMoveDirection = newVelocity.normalized; 
        rb.velocity = velocity;

        RotatePlayer();

        // Update velocity and movement state after handling collision
        velocity = newVelocity;
    }

    private void CalculateMovementState(float inputMagnitude, Vector2 inputClamped)
    {
        if (inputMagnitude > 0.05f)
        {
            // lastMoveDirection = inputClamped;
            movementState = MovementState.Moving;
        }
        else
        {
            movementState = MovementState.Idle;
        }
    }

    private Vector2 CalculateNewVelocity(Vector2 inputClamped)
    {
        float currentLerpSpeed = player.IsInWater() ? moveLerpSpeed : moveLerpSpeed * 0.66f;
        Vector2 newVelocity = player.IsInWater() ? speed * inputClamped : Physics2D.gravity ;
        return Vector2.Lerp(velocity, newVelocity, currentLerpSpeed);
    }

    private void RotatePlayer()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg);
        Quaternion currentRotation = Quaternion.Euler(0, 0, rb.rotation);
        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationLerpSpeed);
        rb.MoveRotation(newRotation.eulerAngles.z);
    }
    
    public MovementState GetMovementState()
    {
        return movementState;
    }
    
    public Vector2 GetVelocity()
    {
        return velocity;
    }
    
    public Vector2 GetPosition()
    {
        return rb.position;
    }
    
    private IEnumerator UpdateDepth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            int depthFlooredInt = Mathf.FloorToInt(rb.position.y - waterLevel.position.y);
            depthMeter.SetDepth(depthFlooredInt);
            player.SetCurrentDepth(depthFlooredInt);
        }
    }
}
