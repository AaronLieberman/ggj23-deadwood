using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningRadius : MonoBehaviour
{
    Collider2D _radius;
    SpriteRenderer _sprite;

    bool _isSummoning = false;

    public PlayerController Player;

    public List<Triggerable> targets = new List<Triggerable>();

    private void Awake()
    {
        _radius = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.enabled = false;


        Player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (PlayerResources.Instance.isAlive && Input.GetButton("Activate") && Player.IsOnGround())
        {
            if (!_isSummoning)
            {
                StartSummoning();
            }
        }
        else
        {
            if (_isSummoning)
            {
                StopSummoning();
            }
        }
    }

    void StartSummoning()
    {
        _isSummoning = true;
        _sprite.enabled = true;
        for (int i=0; i < targets.Count; i++)
        {
            targets[i].StartSummoning();
        }
    }

    void StopSummoning()
    {
        _isSummoning = false;
        _sprite.enabled = false;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].CancelSummoning();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponentInParent<Triggerable>();
        if (other)
        {
            targets.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponentInParent<Triggerable>();
        if (other)
        {
            targets.Remove(other);
        }
    }
}
