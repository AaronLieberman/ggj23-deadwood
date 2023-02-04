using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum MovementState { Idle, Running, Jumping }

    [SerializeField] float JumpSpeed = 14;
    [SerializeField] float MovementSpeed = 6;
    [SerializeField] float GravityScale = 3;

    [SerializeField] LayerMask JumpableGround = 3;

    MovementState _state = MovementState.Idle;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // totally unnescessary to set this in code but simplifies setup for the time being
        _rigidBody.gravityScale = GravityScale;

        float h = Input.GetAxisRaw("Horizontal");
        _rigidBody.velocity = new Vector3(h * MovementSpeed, _rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsOnGround())
        {
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, JumpSpeed);
        }
        else if (h != 0)
        {
            _state = MovementState.Running;
        }
        else
        {
            _state = MovementState.Idle;
        }
        
        if (_rigidBody.velocity.y > 0.1f)
        {
            _state = MovementState.Jumping;
        }
        else if (_rigidBody.velocity.y < -0.1f)
        {
            _state = MovementState.Jumping;
        }

        _animator.SetInteger("state", (int)_state);

        if (h != 0)
        {
            _spriteRenderer.flipX = h < 0;
        }
    }

    bool IsOnGround()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, JumpableGround);
    }
}
