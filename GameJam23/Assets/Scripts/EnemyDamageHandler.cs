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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timeSinceLastDamage <= Time.time)
        {
            if (interactionTags.Contains(collision.tag))
                enemyResources.Damage();
            timeSinceLastDamage = Time.time + damageCooldown;
        }
    }
}
