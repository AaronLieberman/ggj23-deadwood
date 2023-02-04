using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningRadius : MonoBehaviour
{
    Collider2D _radius;

    bool _isSummoning = false;

    public PlayerController Player;

    public List<Triggerable> targets = new List<Triggerable>();

    private void Awake()
    {
        _radius = GetComponent<Collider2D>();
        Player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButton("Activate") && Player.IsOnGround())
        {
            if (!_isSummoning)
            {
                StartSummoning();
            }
        }
        else
        {
            _isSummoning = false;
            for (int i=0; i< targets.Count; i++)
            {
                targets[i].CancelSummoning();
            }
        }


    }

    void StartSummoning()
    {
        _isSummoning = true;
        for (int i=0; i < targets.Count; i++)
        {
            targets[i].StartSummoning();
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
