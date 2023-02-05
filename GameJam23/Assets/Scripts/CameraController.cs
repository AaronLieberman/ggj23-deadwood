using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform Player;

    [SerializeField] float PlayerOffset = 4;

    bool _firstFrame = true;
    float _initialY;

    void Update()
    {
        // do this the first frame because otherwise Player may not have been initialized yet
        if (_firstFrame)
        {
            _initialY = Player.transform.position.y;
            _firstFrame = false;
        }

        transform.position = new Vector3(
            Player.transform.position.x,
            _initialY + (Player.transform.position.y + PlayerOffset - _initialY) / 2,
            Player.transform.position.z - 10);
    }
}
