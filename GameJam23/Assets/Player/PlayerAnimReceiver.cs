using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimReceiver : MonoBehaviour
{
    public void DashComplete()
    {
        transform.parent.GetComponent<PlayerController>().DashComplete();
    }
}
