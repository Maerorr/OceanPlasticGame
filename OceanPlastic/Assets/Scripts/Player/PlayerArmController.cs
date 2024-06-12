using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArmController : MonoBehaviour
{
    private PlayerInput playerInput;
    
    [SerializeField]
    private Transform armTransform;

    private Vector2 rawAimInput;

    private Vector3 initialPosition;

    public SpriteRenderer vacuumSprite;
    public SpriteRenderer forceCannonSprite;

    [SerializeField] 
    private Transform armNonFlipXTransform;

    [SerializeField, Range(0f, 1f)] 
    private float armLerp = 0.001f;
    bool previousFlipX = true;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        initialPosition = armTransform.localPosition;
    }

    public void SetArmPivotPosition(bool flipX)
    {
        
        if (flipX)
        {
            armTransform.localPosition = initialPosition;
            FlipToolSprites(false);
        }
        else
        {
            armTransform.localPosition = armNonFlipXTransform.localPosition;
            FlipToolSprites(true);
        }
        
        if (rawAimInput.magnitude > 0.05f)
        {
            previousFlipX = flipX;
            return;
        }
        if (previousFlipX != flipX)
        {
            Vector3 rotation = armTransform.rotation.eulerAngles;
            float newZ = - 180f - rotation.z;
            armTransform.rotation = Quaternion.Euler(rotation.x, rotation.y, newZ);
        }
        previousFlipX = flipX;
    }
    
    void Update()
    {
        rawAimInput = playerInput.actions["Aim"].ReadValue<Vector2>();
        
        if (rawAimInput.magnitude > 0.05f)
        {
            float angle = Mathf.Atan2(rawAimInput.y, rawAimInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            armTransform.rotation = Quaternion.Lerp(armTransform.rotation, targetRotation, armLerp);
        }
    }
    
    void FlipToolSprites(bool flipY)
    {
        vacuumSprite.flipY = flipY;
        forceCannonSprite.flipY = flipY;
    }
}
