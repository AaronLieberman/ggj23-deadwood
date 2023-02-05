using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDamageHandler : MonoBehaviour
{
    [SerializeField]
    private SummonResources summonResources;
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
            summonResources.Damage();
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
