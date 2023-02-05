using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SummonResources))]
public class SummonDeathHandler : MonoBehaviour
{
    private SummonResources summonResources;

    private void Awake()
    {
        summonResources = GetComponent<SummonResources>();
    }

    private void Update()
    {
        if (summonResources.Health <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {
        SpawnPrefab spawnPrefab = GetComponent<SpawnPrefab>();
        if (spawnPrefab != null)
            spawnPrefab.Spawn();

        Destroy(gameObject);
    }
}
