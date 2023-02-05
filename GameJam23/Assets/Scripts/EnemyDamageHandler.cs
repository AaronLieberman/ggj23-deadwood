using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    [SerializeField]
    EnemyResources enemyResources;
    
    [SerializeField]
    List<string> interactionTags;
    
    [SerializeField]
    float damageCooldown = 1.5f;

    float timeSinceLastDamage = float.MaxValue;

    bool colliding;

    public bool InHurtState { get; private set; }

    private void FixedUpdate()
    {
        if (colliding && timeSinceLastDamage <= Time.time)
        {
            enemyResources.Damage();
            timeSinceLastDamage = Time.time + damageCooldown;
        }

        InHurtState = timeSinceLastDamage <= Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
        {
            colliding = true;

            Vector3 collisionDir = collision.transform.position - transform.position;
            Vector3 impulseDir = new Vector3(-collisionDir.x, 0.1f, 0).normalized;

            GetComponentInChildren<Rigidbody2D>().velocity = impulseDir * 5;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
            colliding = false;
    }
}
