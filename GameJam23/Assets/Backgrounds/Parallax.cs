using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float ParallaxEffect = 0;
    private Vector2 _startPos;
    private float _length;

    void Start()
    {
        _startPos = transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        Vector2 dist = Camera.main.transform.position * ParallaxEffect;

        transform.position = new Vector3(_startPos.x + dist.x, _startPos.y + dist.y, transform.position.z);
    }
}
