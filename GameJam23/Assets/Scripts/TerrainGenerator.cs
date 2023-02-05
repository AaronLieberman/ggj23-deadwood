using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject starterPrefab;

    [SerializeField]
    private int starterPrefabWidth;

    [SerializeField]
    private GameObject endingPrefab;

    [SerializeField]
    private int endingPrefabWidth;

    [SerializeField]
    private GameObject terrainPrefab;

    [SerializeField]
    private float groundHeight;

    [SerializeField]
    private int groundWidth;

    [SerializeField]
    private int groundChunks;
    
    [SerializeField]
    private GameObject[] platformPrefabs;

    [SerializeField]
    private int[] platformWidths;

    [SerializeField]
    private GameObject[] groundBuriedPrefabs;

    [SerializeField]
    private GameObject[] platformBuriedPrefabs;

    [SerializeField]
    private GameObject[] sceneryPrefabs;

    [SerializeField]
    private GameObject[] backdropPrefabs;

    [SerializeField]
    private float platformSpacing;

    [SerializeField]
    private int platformLayers;

    [SerializeField]
    private int platformsPerLayer;

    [SerializeField]
    private int chanceBuriedPerPlatformTile;

    [SerializeField]
    private int chanceBuriedPerGroundTile;

    [SerializeField]
    private GameObject[] flyingEnemyPrefabs;

    [SerializeField]
    private int avgFlyingEnemiesPerChunkStart;

    [SerializeField]
    private int avgFlyingEnemiesPerChunkEnd;

    [SerializeField]
    private GameObject[] groundEnemyPrefabs;

    [SerializeField]
    private int chanceGroundEnemyPerTileStart;

    [SerializeField]
    private int chanceGroundEnemyPerTileEnd;

    private int pSectionLength;

    // Start is called before the first frame update
    void Start()
    {
        CreateGround();
        CreatePlatforms();
    }

    void CreateGround()
    {
        if (starterPrefab != null)
        {
            Instantiate(starterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        int initOffset = starterPrefabWidth / 2 + groundWidth / 2;
        Instantiate(terrainPrefab, new Vector3(initOffset, 0, 0), Quaternion.identity);
        int range = 0;
        System.Random rand = new System.Random();
        for (int i = 0; i < groundChunks; ++i)
        {
            for(int j = 0; j < groundWidth; ++j)
            {
                if(rand.Next(0, 100) < chanceBuriedPerGroundTile)
                {
                    Instantiate(groundBuriedPrefabs[rand.Next(0, groundBuriedPrefabs.Length)], new Vector3(initOffset + range - (groundWidth / 2) + j, groundHeight + 0.75f, 0), Quaternion.identity);
                }
                var interpChance = Math.Round(chanceGroundEnemyPerTileStart + (chanceGroundEnemyPerTileEnd - chanceGroundEnemyPerTileStart) * (range - groundWidth / 2.0 + j) / (groundWidth * groundChunks));
                if (rand.Next(0, 100) < interpChance)
                {
                    Instantiate(groundEnemyPrefabs[rand.Next(0, groundEnemyPrefabs.Length - 1)], new Vector3(initOffset + range - (groundWidth / 2) + j, groundHeight + 1, 0), Quaternion.identity);
                }
            }
            int numBirds = (int)Math.Round(avgFlyingEnemiesPerChunkStart + (avgFlyingEnemiesPerChunkEnd - avgFlyingEnemiesPerChunkStart) * (i * 1.0 / groundChunks));
            for (int j = 0; j < numBirds; ++j)
            {
                Instantiate(flyingEnemyPrefabs[rand.Next(0, flyingEnemyPrefabs.Length - 1)], new Vector3(initOffset + range - groundWidth / 2 + rand.Next(0, groundWidth), rand.Next(0, 12), 0), Quaternion.identity);
            }
            range += groundWidth;
            Instantiate(terrainPrefab, new Vector3(initOffset + range, 0, 0), Quaternion.identity);
        }
        if (endingPrefab != null)
        {
            Instantiate(endingPrefab, new Vector3(initOffset + range, 0, 0), Quaternion.identity);
        }
        range = rand.Next(3, 20);
        pSectionLength = groundChunks * groundWidth;
        if (sceneryPrefabs.Length > 0)
        {
            while (range < pSectionLength)
            {
                var i = rand.Next(0, sceneryPrefabs.Length);
                Instantiate(sceneryPrefabs[i], new Vector3(initOffset + range, groundHeight, 0), Quaternion.identity);
                Renderer render = sceneryPrefabs[i].GetComponentInChildren(typeof(Renderer)) as Renderer;
                range += (int)Math.Round(render.bounds.size.x);
                range += rand.Next(3, 20);
            }
        }
        range = rand.Next(2, 5);
        if (backdropPrefabs.Length > 0)
        {
            while (range < pSectionLength)
            {
                var i = rand.Next(0, backdropPrefabs.Length);
                Instantiate(backdropPrefabs[i], new Vector3(initOffset + range, groundHeight, 0), Quaternion.identity);
                Renderer render = backdropPrefabs[i].GetComponentInChildren(typeof(Renderer)) as Renderer;
                range += (int)Math.Round(render.bounds.size.x);
                range += rand.Next(2, 5);
            }
        }
    }

    struct PlatData
    {
        public int width;
        public int center;
    }

    void CreatePlatforms()
    {
        List<List<PlatData>> platforms = new List<List<PlatData>>();
        for (int i = 0; i < platformLayers; ++i)
        {
            platforms.Add(new List<PlatData>());
            for (int j = 0; j < platformsPerLayer; ++j)
            {
                //select platform type
                System.Random rand = new System.Random();
                var pi = rand.Next(0, platformPrefabs.Length);
                var platform = platformPrefabs[pi];
                var platformWidth = platformWidths[pi];
                bool collides;
                bool stacks; //used to check if you can jump to it from the level below\
                int center = 0;
                int tries = 0;
                do
                {
                    ++tries;
                    collides = false;
                    stacks = (i == 0);
                    center = rand.Next(platformWidth / 2, pSectionLength - platformWidth / 2);
                    foreach (var p in platforms[i])
                    {
                        if(Math.Abs(p.center - center) < (platformWidth + p.width) / 2) collides = true;
                    }
                    if (i > 0)
                    {
                        foreach (var p in platforms[i - 1])
                        {
                            if(Math.Abs(p.center - center) < (platformWidth + p.width + 4) / 2) stacks = true; 
                        }
                    }
                } while ((collides || !stacks));
                if (tries < 10)
                {
                    PlatData plat = new PlatData();
                    plat.width = platformWidth;
                    plat.center = center;
                    platforms[i].Add(plat);
                    Instantiate(platform, new Vector3(center + starterPrefabWidth / 2, groundHeight + platformSpacing * (1 + i), 0), Quaternion.identity);
                    for (int k = 2; k < platformWidth - 2; ++k)
                    {
                        if (rand.Next(0, 100) < chanceBuriedPerPlatformTile)
                        {
                            Instantiate(platformBuriedPrefabs[rand.Next(0, platformBuriedPrefabs.Length - 1)], new Vector3(center + (starterPrefabWidth / 2) - (platformWidth / 2) + k + 0.5f, groundHeight + platformSpacing * (1 + i) + 0.75f, 0), Quaternion.identity);
                        }
                        var interpChance = Math.Round(chanceGroundEnemyPerTileStart + (chanceGroundEnemyPerTileEnd - chanceGroundEnemyPerTileStart) * center * 1.0 / pSectionLength);
                        if (rand.Next(0, 100) < interpChance)
                        {
                            Instantiate(groundEnemyPrefabs[rand.Next(0, groundEnemyPrefabs.Length - 1)], new Vector3(center + (starterPrefabWidth / 2) - (platformWidth / 2) + k + 0.5f, groundHeight + platformSpacing * (1 + i) + 1, 0), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
