using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSummonBehavior : MonoBehaviour
{
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius;
    [SerializeField] private LayerMask whatIsWall;
    private bool hittingWall;

    private Rigidbody2D _rb;
    public float Speed = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var damageHandler = GetComponentInChildren<SummonDamageHandler>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        bool isInHurtState = damageHandler != null && damageHandler.InHurtState;

        if (isInHurtState)
        {
            spriteRenderer.color = new Color(
                1,
                Mathf.PingPong(Time.time * 3, 1),
                Mathf.PingPong(Time.time * 3, 1));
            return;
        }

        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        _rb.velocity = new Vector2(Speed, _rb.velocity.y); //TODO: Give it actual movement
    }

    private void FixedUpdate()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (hittingWall)
            Jump();
    }

    private void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }
}
