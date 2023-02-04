using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    float? _initialY;

    void Update()
    {
        _initialY ??= Player.transform.position.y;

        transform.position = new Vector3(
            Player.transform.position.x,
            _initialY.Value + (Player.transform.position.y - _initialY.Value) / 2,
            Player.transform.position.z - 10);
    }
}
