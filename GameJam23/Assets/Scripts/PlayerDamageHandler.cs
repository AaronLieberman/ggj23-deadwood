using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerResources playerResources;
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
            playerResources.Damage();
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
