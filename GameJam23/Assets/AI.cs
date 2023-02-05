using UnityEngine;

public enum MoveType { SEEK_FLY, SEEK_WALK, SEEK_EDGE_AWARE_WALK, PATROL_INTERVAL_WALK, PATROL_NONINTERVAL_WALK }

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private float enemyScaleSize = 1.5f;
    [SerializeField] private float inverseScale = 1;
    [SerializeField] private float altInverseScale = -1;

    [SerializeField] private Transform target;
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

    private bool moveRight;

    [SerializeField]
    private float rotationInterval;

    private void Start()
    {
        if (moveType == MoveType.PATROL_INTERVAL_WALK)
            InvokeRepeating("ForceEnemyRotate", 0f, rotationInterval);
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // Handle rotating object based on local scale position.
        if (transform.localScale.x < 0f)
            altInverseScale = -inverseScale;
        else if (transform.localScale.x > 0f)
            altInverseScale = 1;

        if (moveType == MoveType.SEEK_FLY)
        {
            MoveSeekingFlyingEnemy();
        }
        else if (moveType == MoveType.SEEK_WALK)
        {
            MoveSeekingWalkingEnemy();
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
        if (GameObject.FindGameObjectWithTag("Player") != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void ForceEnemyRotate()
    {
        moveRight = !moveRight;
    }
}
