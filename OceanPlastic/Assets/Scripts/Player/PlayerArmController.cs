using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArmController : MonoBehaviour
{
    private PlayerInput playerInput;
    
    [SerializeField]
    private Transform armTransform;

    [SerializeField, Range(0f, 1f)] 
    private float armLerp = 0.001f;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    void Update()
    {
        Vector2 input = playerInput.actions["Aim"].ReadValue<Vector2>();
        
        if (input.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            armTransform.rotation = Quaternion.Lerp(armTransform.rotation, targetRotation, armLerp);
        }
    }
}
