using UnityEngine;

public class EnemyHop : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 500f;

    private void Start()
    {
        InvokeRepeating("ApplyJump", 0f, 2f);
    }

    private void ApplyJump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
    }
}
