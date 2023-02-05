using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSummonBehavior : MonoBehaviour
{
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
}
