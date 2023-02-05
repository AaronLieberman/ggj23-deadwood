using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MovementState { Idle, Running, Jumping, Falling, Dashing, Channelling }

    [SerializeField] float JumpSpeed = 14;
    [SerializeField] float MovementSpeed = 6;
    [SerializeField] float DashSpeed = 16;
    [SerializeField] float GravityScale = 3;

    [SerializeField] LayerMask JumpableGround = 3;

    MovementState _state = MovementState.Idle;
    bool _usedDoubleJump = false;
    bool _wasOnGround = false; // Used to track if we should play the landing sfx

    [SerializeField] AudioClip JumpClip;
    [SerializeField] AudioClip LandClip;
    [SerializeField] List<AudioClip> DashClips;
    [SerializeField] List<AudioClip> FootstepClips;
    [SerializeField] float FootstepInterval = 0.15f;
    float _footstepTimer = 0f;

    Rigidbody2D _rigidBody;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    BoxCollider2D _boxCollider;
    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _boxCollider = GetComponentInChildren<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        Camera.main.GetComponent<CameraController>().Player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // totally unnescessary to set this in code but simplifies setup for the time being
        _rigidBody.gravityScale = GravityScale;

        if (PlayerResources.Instance.isAlive && _state != MovementState.Dashing)
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (h != 0)
            {
                _spriteRenderer.flipX = h < 0;
            }

            bool isOnGround = IsOnGround();

            if (isOnGround && !_wasOnGround)
            {
                _audioSource.PlayOneShot(LandClip);
            }

            _wasOnGround = isOnGround;

            // some states are persistent until they complete, otherwise, default to idle unless we have a better
            // state to be in
            switch (_state)
            {
                case MovementState.Dashing:
                    break;
                default:
                    _state = MovementState.Idle;
                    break;
            }

            if (Input.GetButton("Activate") && isOnGround)
            {
                _state = MovementState.Channelling;
            }
            else if (Input.GetButtonDown("Jump") && (isOnGround || !_usedDoubleJump))
            {
                if (isOnGround)
                {
                    _audioSource.PlayOneShot(JumpClip);
                    _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, JumpSpeed);
                }
                else
                {
                    _audioSource.PlayOneShot(DashClips[Random.Range(0,DashClips.Count-1)]);
                    _usedDoubleJump = true;
                    _rigidBody.velocity = new Vector3(
                        _rigidBody.velocity.x + (_spriteRenderer.flipX ? -DashSpeed : DashSpeed),
                        _rigidBody.velocity.y);
                    _state = MovementState.Dashing;
                }
            }
            else if (Mathf.Abs(h) > 0.1f)
            {
                _state = MovementState.Running;

                if (isOnGround)
                {
                    PlayFootsteps();
                }
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
            }

            switch (_state)
            {
                case MovementState.Idle:
                case MovementState.Jumping:
                case MovementState.Running:
                case MovementState.Falling:
                    _rigidBody.velocity = new Vector3(h * MovementSpeed, _rigidBody.velocity.y);
                    break;
                case MovementState.Channelling:
                    _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y);
                    break;
            }

            if (isOnGround)
            {
                _usedDoubleJump = false;
            }
        }

        _animator.SetInteger("state", (int)_state);
    }

    void PlayFootsteps()
    {
        _footstepTimer += Time.deltaTime;
        if (_footstepTimer >= FootstepInterval)
        {
            _footstepTimer = 0f;
            _audioSource.PlayOneShot(FootstepClips[Random.Range(0, FootstepClips.Count - 1)],0.5f);
        }
    }

    public bool IsOnGround()
    {
        return Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, JumpableGround);
    }

    public void DashComplete()
    {
        _state = MovementState.Falling;
    }
}
