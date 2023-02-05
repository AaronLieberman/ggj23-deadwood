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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timeSinceLastDamage <= Time.time)
        {
            if (interactionTags.Contains(collision.tag))
                playerResources.Damage();
            timeSinceLastDamage = Time.time + damageCooldown;
        }
    }
}
