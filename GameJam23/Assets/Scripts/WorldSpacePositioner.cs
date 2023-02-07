using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpacePositioner : MonoBehaviour
{
    [SerializeField] GameObject Target;

    void Update()
    {
        //var targetPos = Camera.main.WorldToScreenPoint(Target.transform.position);
        //transform.position = targetPos;
        transform.position = Target.transform.position;
    }
}
