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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timeSinceLastDamage <= Time.time)
        {
            if (interactionTags.Contains(collision.tag))
                summonResources.Damage();
            timeSinceLastDamage = Time.time + damageCooldown;
        }
    }
}
