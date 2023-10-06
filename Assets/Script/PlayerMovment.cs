using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

// Basic Platformer
public class Step3Movement : MonoBehaviour
{
    private float _moveSpeed = 2f;
    public float jumpSpeed = 5f;
    public float currentJumpSpeed = 0f;
    public float chargeTime = 0f;
    private float _jumpSpeedBoost;
    public bool isGrounded;
    public bool isSlippin;
    public bool isJumping;
    public float yVelocity;
    
    public LayerMask whatIsGround;
    

    private InputManager _input;
    private Rigidbody2D _rigidbody2D;
    private PlayerAnimator _playerAnimation;
    private bool _clamp;

    private void Start()
    {
        _input = GetComponent<InputManager>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimator>();
    }
    
    private void Update()
    {
        // Are we on ground?
        isGrounded = Physics2D.Raycast(new Vector2(transform.position.x - 0.1f, transform.position.y), Vector2.down, 1.1f, whatIsGround)|| Physics2D.Raycast(new Vector2(transform.position.x + 0.1f, transform.position.y), Vector2.down, 1.1f, whatIsGround);
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, whatIsGround);
        Debug.DrawRay(new Vector2(transform.position.x -0.1f, transform.position.y), Vector3.down, Color.cyan);
        Debug.DrawRay(new Vector2(transform.position.x + 0.1f, transform.position.y), Vector3.down, Color.cyan);

        
        if (_input.jumpPressed)
        {
            currentJumpSpeed = jumpSpeed;
            chargeTime = Time.time;
            _clamp = true;
            _moveSpeed = 5f;
            isJumping = true;
        }
        
        if (_input.jumpHeld)
        {
            if (Time.time < chargeTime + 2)
            {
                currentJumpSpeed = jumpSpeed + (Time.time - chargeTime) * 3.8f;
            }
        }
        
        // Jump
        if (_input.jumpReleased && isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, currentJumpSpeed);
            _jumpSpeedBoost = Time.time + 0.5f;
            _clamp = false;
            
        }

        if (_jumpSpeedBoost < Time.time && !_clamp)
        {
            _moveSpeed = 2;
            if (isGrounded)
            {
                isJumping = false;
            }
        }
        
        _playerAnimation.UpdateAnimation(_rigidbody2D.velocity, isGrounded, _input.moveDirection);
        yVelocity = _rigidbody2D.velocity.y;
        
        if (hit.transform == null) return;
       
        if (hit.transform.CompareTag("Ice") && (_moveSpeed != 5f))
        {
            isSlippin = true;
            
        }
        else
        {
            isSlippin = false;
        }
    }

    private void FixedUpdate()
    {
        // Movement
        if (!_clamp && isGrounded && !isSlippin)
        {
            _rigidbody2D.velocity = new Vector2(_input.moveDirection.x * _moveSpeed, _rigidbody2D.velocity.y);   
        }
    }
}   