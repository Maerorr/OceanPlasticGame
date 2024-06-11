using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    public void PlayerToolSpriteSheet()
    {
        animator.SetBool("isToolEquipped", true);
    }
    
    public void PlayerNoToolSpriteSheet()
    {
        animator.SetBool("isToolEquipped", false);
    }
    
}
