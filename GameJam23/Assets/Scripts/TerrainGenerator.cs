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
    private Platform[] platformPresets;

    [SerializeField]
    private GameObject[] buriedPresets;

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
    private int avgEnemiesPerChunkStart;

    [SerializeField]
    private int avgEnemiesPerChunkEnd;

    private int pSectionLength;

    // Start is called before the first frame update
    void Start()
    {
        CreateGround();
        CreatePlatforms();
    }

    void CreateGround()
    {
        Instantiate(starterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        int initOffset = starterPrefabWidth / 2 + groundWidth / 2;
        Instantiate(terrainPrefab, new Vector3(initOffset, 0, 0), Quaternion.identity);
        int range = 0;
        for (int i = 0; i < groundChunks; ++i)
        {
            System.Random rand = new System.Random();
            for(int j = 0; j < groundWidth; ++j)
            {
                if(rand.Next(0, 100) < chanceBuriedPerGroundTile)
                {
                    Instantiate(buriedPresets[rand.Next(0, buriedPresets.Length)], new Vector3(initOffset + range - (groundWidth / 2) + j, groundHeight - 0.5f, 0), Quaternion.identity);
                }
            }
            range += groundWidth;
            Instantiate(terrainPrefab, new Vector3(initOffset + range, 0, 0), Quaternion.identity);
        }
        pSectionLength = groundChunks * groundWidth;
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
                var platform = platformPresets[rand.Next(0, platformPresets.Length)];
                bool collides;
                bool stacks; //used to check if you can jump to it from the level below\
                int center = 0;
                int tries = 0;
                do
                {
                    ++tries;
                    collides = false;
                    stacks = (i == 0);
                    center = rand.Next(platform.width / 2, pSectionLength - platform.width / 2);
                    foreach (var p in platforms[i])
                    {
                        if(Math.Abs(p.center - center) < (platform.width + p.width) / 2) collides = true;
                    }
                    if (i > 0)
                    {
                        foreach (var p in platforms[i - 1])
                        {
                            if(Math.Abs(p.center - center) < (platform.width + p.width + 4) / 2) stacks = true; 
                        }
                    }
                } while ((collides || !stacks));
                if (tries < 10)
                {
                    PlatData plat = new PlatData();
                    plat.width = platform.width;
                    plat.center = center;
                    platforms[i].Add(plat);
                    Instantiate(platform, new Vector3(center + starterPrefabWidth / 2, groundHeight + platformSpacing * (1 + i), 0), Quaternion.identity);
                    for (int k = 0; k < platform.width; ++k)
                    {
                        if (rand.Next(0, 100) < chanceBuriedPerPlatformTile)
                        {
                            Instantiate(buriedPresets[rand.Next(0, buriedPresets.Length - 1)], new Vector3(center + (starterPrefabWidth / 2) - (platform.width / 2) + k, groundHeight + platformSpacing * (1 + i) - 0.5f, 0), Quaternion.identity);
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
