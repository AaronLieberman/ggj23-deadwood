using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    [SerializeField]
    private EnemyResources enemyResources;
    [SerializeField]
    private List<string> interactionTags;
    [SerializeField]
    private float damageCooldown = 5f;
    private float timeSinceLastDamage;
    private bool damageable = false;

    private void FixedUpdate()
    {
        if (damageable && timeSinceLastDamage <= Time.time)
        {
            enemyResources.Damage();
            timeSinceLastDamage = Time.time + damageCooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
            damageable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactionTags.Contains(collision.tag))
            damageable = false;
    }
}
