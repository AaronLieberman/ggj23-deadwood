using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    private bool _playerTargeted = false;

    public UnityEvent Activate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _playerTargeted = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }

        _playerTargeted = false;
    }

    private void Update()
    {
        if (_playerTargeted && Input.GetButtonDown("Activate"))
        {
            Debug.Log($"Activated {gameObject.name}");
            Activate?.Invoke();
        }
    }
}
