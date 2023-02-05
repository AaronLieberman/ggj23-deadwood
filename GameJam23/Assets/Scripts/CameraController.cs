using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    bool _firstFrame = true;
    float _initialY;
    float _offsetY;

    void Update()
    {
        // do this the first frame because otherwise Player may not have been initialized yet
        if (_firstFrame)
        {
            _initialY = Player.transform.position.y;
            _offsetY = transform.position.y - _initialY;
            _firstFrame = false;
        }

        transform.position = new Vector3(
            Player.transform.position.x,
            _initialY + (Player.transform.position.y + _offsetY - _initialY) / 2,
            Player.transform.position.z - 10);
    }
}
