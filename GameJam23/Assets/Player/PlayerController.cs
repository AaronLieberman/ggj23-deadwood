using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MovementState { Idle, Running, Jumping, Falling, Dashing }

    [SerializeField] float JumpSpeed = 14;
    [SerializeField] float MovementSpeed = 6;
    [SerializeField] float DashSpeed = 16;
    [SerializeField] float GravityScale = 3;

    [SerializeField] LayerMask JumpableGround = 3;

    MovementState _state = MovementState.Idle;
    bool _usedDoubleJump = false;

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

        Camera.main.GetComponent<CameraController>().Player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // totally unnescessary to set this in code but simplifies setup for the time being
        _rigidBody.gravityScale = GravityScale;

        float h = Input.GetAxisRaw("Horizontal");
        if (_state != MovementState.Dashing)
        {
            _rigidBody.velocity = new Vector3(h * MovementSpeed, _rigidBody.velocity.y);

            if (h != 0)
            {
                _spriteRenderer.flipX = h < 0;
            }

            bool isOnGround = IsOnGround();

            if (Input.GetButtonDown("Jump") && (isOnGround || !_usedDoubleJump))
            {
                if (isOnGround)
                {
                    _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, JumpSpeed);
                }
                else
                {
                    _usedDoubleJump = true;
                    _rigidBody.velocity = new Vector3(
                        _rigidBody.velocity.x + (_spriteRenderer.flipX ? -DashSpeed : DashSpeed),
                        _rigidBody.velocity.y);
                    _state = MovementState.Dashing;
                }
            }
            else if (h != 0)
            {
                _state = MovementState.Running;
            }

            if (_state != MovementState.Dashing)
            {
                if (_rigidBody.velocity.y > 0.1f)
                {
                    _state = MovementState.Jumping;
                }
                else if (_rigidBody.velocity.y < -0.1f)
                {
                    _state = MovementState.Falling;
                }
                else
                {
                    _state = MovementState.Idle;
                }
            }

            if (isOnGround)
            {
                _usedDoubleJump = false;
            }
        }

        _animator.SetInteger("state", (int)_state);
    }

    bool IsOnGround()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, JumpableGround);
    }

    public void DashComplete()
    {
        _state = MovementState.Falling;
    }
}
