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
    [SerializeField] private float speed = 5f;
    [SerializeField, Range(0f, 1f)] private float lerpSpeed = 0.1f;
    [SerializeField] private ContactFilter2D contactFilter;
    private List<RaycastHit2D> hitBuffer = new List<RaycastHit2D>();
    private Vector2 lastMoveDirection = Vector2.zero;
    private MovementState movementState = MovementState.Idle;
    private Player player;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    // private void Move()
    // {
    //     Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
    //
    //     var inputClamped = Vector2.ClampMagnitude(input, 1f);
    //     if (input.magnitude > 0.05f)
    //     {
    //         lastMoveDirection = inputClamped;
    //         movementState = MovementState.Moving;
    //     }
    //     else
    //     {
    //         movementState = MovementState.Idle;
    //     }
    //     Vector2 newVelocity;
    //     float currentLerpSpeed = this.lerpSpeed;
    //     if (player.IsInWater())
    //     {
    //         newVelocity = speed * Time.fixedDeltaTime * inputClamped;
    //     }
    //     else
    //     {
    //         // apply gravity manually
    //         newVelocity = Physics2D.gravity * Time.fixedDeltaTime;
    //         currentLerpSpeed = lerpSpeed* 0.66f;
    //     }
    //         
    //     velocity = Vector2.Lerp(velocity, newVelocity, currentLerpSpeed);
    //
    //     if (!TryMove(velocity.x, velocity.y))
    //     {
    //         bool[] flags = new bool[2];
    //         flags[0] = TryMove(velocity.x, 0f);
    //         flags[1] = TryMove(0f, velocity.y);
    //         if (!flags[0] && !flags[1])
    //         {
    //             velocity = Vector2.zero;
    //         }
    //     }
    //     Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg);
    //     rb.MoveRotation(rotation);
    // }
    
    private void Move()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 inputClamped = Vector2.ClampMagnitude(input, 1f);
        CalculateMovementState(inputClamped.magnitude, inputClamped);

        Vector2 newVelocity = CalculateNewVelocity(inputClamped);

        if (!TryMove(newVelocity.x, newVelocity.y))
        {
            HandleCollision();
        }

        RotatePlayer();

        // Update velocity and movement state after handling collision
        velocity = newVelocity;
    }

    private void CalculateMovementState(float inputMagnitude, Vector2 inputClamped)
    {
        if (inputMagnitude > 0.05f)
        {
            lastMoveDirection = inputClamped;
            movementState = MovementState.Moving;
        }
        else
        {
            movementState = MovementState.Idle;
        }
    }

    private Vector2 CalculateNewVelocity(Vector2 inputClamped)
    {
        float currentLerpSpeed = player.IsInWater() ? lerpSpeed : lerpSpeed * 0.66f;
        Vector2 newVelocity = player.IsInWater() ? speed * Time.fixedDeltaTime * inputClamped : Physics2D.gravity * Time.fixedDeltaTime;
        return Vector2.Lerp(velocity, newVelocity, currentLerpSpeed);
    }

    private void HandleCollision()
    {
        bool[] flags = new bool[2];
        flags[0] = TryMove(velocity.x, 0f);
        flags[1] = TryMove(0f, velocity.y);
        if (!flags[0] && !flags[1])
        {
            velocity = Vector2.zero;
        }
    }

    private void RotatePlayer()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg);
        Quaternion currentRotation = Quaternion.Euler(0, 0, rb.rotation);
        Quaternion newRotation = Quaternion.Lerp(currentRotation, targetRotation, lerpSpeed);
        rb.MoveRotation(newRotation.eulerAngles.z);
    }
    
    private bool TryMove(float x, float y)
    {
        var move = new Vector2(x, y);
        var hits = rb.Cast(
            move,
            contactFilter,
            hitBuffer,
            move.magnitude + 0.1f
        );
        
        if (hits != 0)
        {
            string debug = "";
            foreach (var var in hitBuffer)
            {
                debug += var.collider.name + " ";
            }
            Debug.Log(debug);
            return false;
        }
        
        rb.MovePosition(rb.position + move);
        return true;
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered trigger");
    }
}
