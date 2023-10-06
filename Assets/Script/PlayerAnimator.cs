using System;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private Step3Movement _move;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _move = GetComponent<Step3Movement>();
    }
    
    public void UpdateAnimation(Vector2 velocity, bool isGrounded, Vector2 input)
    {
        if (input.x != 0)
        {
            transform.localScale = new Vector3( input.x, transform.localScale.y, 1);
        }

        if (!_move.isJumping)
        {
            if (input.x != 0)
            {
                _animator.SetBool("Walking",true);    
            }
            else
            {
                _animator.SetBool("Walking",false);
            }
            
        }
        else
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                _animator.SetBool("Crouching",true);
                
            }

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                _animator.SetBool("Crouching",false);
            }

            if (velocity.y < 0f)
            {
                _animator.SetBool("Falling",true);
            }
        }
        if (isGrounded)
        {
            _animator.SetBool("Falling",false);
            _animator.SetBool("Landing", true);
        }
        else
        {
            _animator.SetBool("Landing", false);
        }
    }
}