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
        _rb.velocity = new Vector2(Speed, 0f); //TODO: Give it actual movement
    }
}
