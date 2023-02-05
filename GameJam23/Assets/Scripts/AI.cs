using UnityEngine;

public enum MoveType { SEEK_FLY, SEEK_WALK, SEEK_EDGE_AWARE_WALK, PATROL_INTERVAL_WALK, PATROL_NONINTERVAL_WALK, HOP }

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private float enemyScaleSize = 1.5f;
    [SerializeField] private float inverseScale = 1;
    [SerializeField] private float altInverseScale = -1;

    private Transform target;
    [SerializeField] private float agroDist;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private LayerMask whatIsWall;
    private bool hittingWall;

    [SerializeField] private Transform edgeCheck;
    [SerializeField] private float edgeCheckRadius;
    [SerializeField] private LayerMask whatIsNonEdge;
    private bool notOnEdge;

    [SerializeField] private MoveType moveType;

    [SerializeField] private string targetTag = string.Empty;

    private bool moveRight;

    [SerializeField]
    private float rotationInterval;

    enum MovementState { Idle, Jumping, Falling, Move, Hurt }

    Rigidbody2D _rigidBody;
    Animator _animator;
    EnemyDamageHandler _enemyDamageHandler;
    SpriteRenderer _spriteRenderer;
    
    Color _baseColor;

    MovementState _state = MovementState.Idle;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _enemyDamageHandler = GetComponentInChildren<EnemyDamageHandler>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _baseColor = _spriteRenderer.color;

        if (moveType == MoveType.PATROL_INTERVAL_WALK)
            InvokeRepeating("ForceEnemyRotate", 0f, rotationInterval);
    }

    private void FixedUpdate()
    {
        Vector3 prevPos = transform.position;
        MoveEnemy();
        UpdateAnimation(transform.position != prevPos);
    }

    private void MoveEnemy()
    {
        // Handle rotating object based on local scale position.
        if (transform.localScale.x < 0f)
            altInverseScale = -inverseScale;
        else if (transform.localScale.x > 0f)
            altInverseScale = 1;

        bool isInHurtState = _enemyDamageHandler != null && _enemyDamageHandler.InHurtState;

        if (isInHurtState)
        {
            _spriteRenderer.color = new Color(
                _baseColor.r,
                _baseColor.g * Mathf.PingPong(Time.time * 3, 1),
                _baseColor.b * Mathf.PingPong(Time.time * 3, 1));
            return;
        }

        _spriteRenderer.color = _baseColor;

        if (moveType == MoveType.SEEK_FLY)
        {
            MoveSeekingFlyingEnemy();
        }
        else if (moveType == MoveType.SEEK_WALK)
        {
            MoveSeekingWalkingEnemy();
        }
        else if (moveType == MoveType.HOP)
        {
            MoveHoppingEnemy();
        }
        else if (moveType == MoveType.PATROL_NONINTERVAL_WALK)
        {
            MovePatrolNonIntervalEnemy();
        }
        else if (moveType == MoveType.PATROL_INTERVAL_WALK)
        {
            MovePatrolIntervalEnemy();
        }
        else if (moveType == MoveType.SEEK_EDGE_AWARE_WALK)
        {
            MoveEdgeAwareSeekingWalkingEnemy();
        }
    }

    private void MoveSeekingFlyingEnemy()
    {
        if (target != null)
        {
            var toTargetDist = Vector2.Distance(transform.position, target.position);

            if (toTargetDist > agroDist && moveRight)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 1, transform.position.y), speed * Time.deltaTime);
            }
            else if (toTargetDist > agroDist && !moveRight)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 1, transform.position.y), speed * Time.deltaTime);
            }
            if (toTargetDist < agroDist && transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                moveRight = true;
            }
            else if (toTargetDist < agroDist && transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                moveRight = false;
            }
        }
        else
        {
            AttemptFindAndAttachPlayerGameObject();
        }
    }

    private void MoveSeekingWalkingEnemy()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (hittingWall)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

        if (target != null)
        {
            var toTargetDist = Vector2.Distance(transform.position, target.position);

            if (toTargetDist > agroDist && moveRight)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (toTargetDist > agroDist && !moveRight)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (toTargetDist < agroDist && transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                moveRight = true;
            }
            else if (toTargetDist < agroDist && transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
                moveRight = false;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            AttemptFindAndAttachPlayerGameObject();
        }
    }

    private void MoveHoppingEnemy()
    {
        if (target != null)
        {
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > 0.1f)
            {
                var toTargetDist = Vector2.Distance(transform.position, target.position);

                if (toTargetDist > agroDist && moveRight)
                {
                    transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                }
                else if (toTargetDist > agroDist && !moveRight)
                {
                    transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
                }

                if (toTargetDist < agroDist && transform.position.x < target.position.x)
                {
                    transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                    moveRight = true;
                }
                else if (toTargetDist < agroDist && transform.position.x > target.position.x)
                {
                    transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
                    moveRight = false;
                }
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            AttemptFindAndAttachPlayerGameObject();
        }
    }

    private void MoveEdgeAwareSeekingWalkingEnemy()
    {
        if (target != null)
        {
            var toTargetDist = Vector2.Distance(transform.position, target.position);

            if (toTargetDist > agroDist && moveRight)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (toTargetDist > agroDist && !moveRight)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (toTargetDist < agroDist && transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                moveRight = true;
            }
            else if (toTargetDist < agroDist && transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(inverseScale * -enemyScaleSize, enemyScaleSize, 1f);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, GetComponent<Rigidbody2D>().velocity.y);
                moveRight = false;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            AttemptFindAndAttachPlayerGameObject();
        }

        notOnEdge = Physics2D.OverlapCircle(edgeCheck.position, edgeCheckRadius, whatIsNonEdge);
        if (!notOnEdge)
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    private void MovePatrolIntervalEnemy()
    {
        if (moveRight)
        {
            transform.localScale = new Vector3(enemyScaleSize, enemyScaleSize, 1f);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 1, transform.position.y), speed * Time.deltaTime);
        }
        else if (!moveRight)
        {
            transform.localScale = new Vector3(-enemyScaleSize, enemyScaleSize, 1f);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 1, transform.position.y), speed * Time.deltaTime);
        }
    }

    private void MovePatrolNonIntervalEnemy()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (hittingWall)
            ForceEnemyRotate();

        if (moveRight)
        {
            transform.localScale = new Vector3(enemyScaleSize, enemyScaleSize, 1f);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 1, transform.position.y), speed * Time.deltaTime);
        }
        else if (!moveRight)
        {
            transform.localScale = new Vector3(-enemyScaleSize, enemyScaleSize, 1f);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 1, transform.position.y), speed * Time.deltaTime);
        }
    }

    private void AttemptFindAndAttachPlayerGameObject()
    {
        if (GameObject.FindGameObjectWithTag(targetTag) != null)
        {
            GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);
            Transform closestTarget = null;
            foreach(GameObject targetObject in targetObjects)
            {
                if (closestTarget == null)
                    closestTarget = targetObject.transform;
                else if (Vector2.Distance(transform.position, targetObject.transform.position) < Vector2.Distance(transform.position, closestTarget.position))
                    closestTarget = targetObject.transform;
            }
            target = closestTarget;
        }
            
    }

    private void ForceEnemyRotate()
    {
        moveRight = !moveRight;
    }

    void UpdateAnimation(bool movedThisFrame)
    {
        if (_animator == null) return;

        if (moveType == MoveType.HOP)
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
        else
        {
            _state = movedThisFrame || Mathf.Abs(_rigidBody.velocity.x) > 0.1f ? MovementState.Move : MovementState.Idle;
        }

        _animator.SetInteger("state", (int)_state);
    }
}
