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

    public bool InHurtState { get; private set; }

    private void FixedUpdate()
    {
        if (damageable && timeSinceLastDamage <= Time.time)
        {
            StartCoroutine(ApplyHurt());
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

    private IEnumerator ApplyHurt()
    {
        InHurtState = true;

        yield return new WaitForSeconds(3f);

        InHurtState = false;
    }
}
