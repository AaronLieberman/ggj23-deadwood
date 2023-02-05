using UnityEngine;

public class AIHop : MonoBehaviour
{
    [SerializeField]
    float jumpForce = 500f;

    void Start()
    {
        InvokeRepeating("ApplyJump", 0f, 2f);
    }

    void ApplyJump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }
}
