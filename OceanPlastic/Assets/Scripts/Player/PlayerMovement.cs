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
    private float joystickMagnitude;
    private Vector2 rawInput;
    private Vector2 lastNonZeroInput;
    
    [SerializeField] 
    private float speed = 5f;
    private float speedModifier = 1f;
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
    private SpriteRenderer playerCharacterRenderer;
    
    private PlayerArmController armController;
    
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
        armController = GetComponent<PlayerArmController>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        rawInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (rawInput.magnitude > 0.05f)
        {
            lastNonZeroInput = rawInput;
        }
        Vector2 inputClamped = Vector2.ClampMagnitude(rawInput, 1f);
        joystickMagnitude = rawInput.magnitude;
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
        Vector2 newVelocity = player.IsInWater() ? (speed * speedModifier) * inputClamped : Physics2D.gravity ;
        return Vector2.Lerp(velocity, newVelocity, currentLerpSpeed);
    }

    private void RotatePlayer()
    {
        // old rotation
        // Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg);
        // Quaternion currentRotation = Quaternion.Euler(0, 0, rb.rotation);
        // Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationLerpSpeed);
        // rb.MoveRotation(newRotation.eulerAngles.z);
        
        // sprite rotation
        if (lastNonZeroInput.x > 0)
        {
            playerCharacterRenderer.flipX = true;
            armController.SetArmPivotPosition(true);
        }
        else
        {
            playerCharacterRenderer.flipX = false;
            armController.SetArmPivotPosition(false);
        }
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

    public float GetInputMagnitude()
    {
        return joystickMagnitude;
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

    public void SetSpeedModifier(float mod)
    {
        speedModifier = mod;
    }
}
