using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Links")]
    public Level level;
    [Header("Generation")]
    [SerializeField] Transform biomeContainer;
    [SerializeField] Biome startBiome;
    [SerializeField] Biome endBiome;
    [SerializeField] List<Biome> biomeTemplates;

    public int startPosition = -57;
    public int numberOfBiome = 3;
    public int biomeSpacing;


    [Header("Status")]
    [SerializeField] Biome previousBiome = null;
    [SerializeField] Biome currentBiome = null;
    [SerializeField] List<Biome> biomes;
    [SerializeField] int seed;

    public Biome CurrentBiome
    {
        get { return currentBiome; }
        set
        {
            if (currentBiome != value)
            {
                previousBiome = currentBiome;
                currentBiome = value;

                if(previousBiome!=null && currentBiome == startBiome)
                {
                    level.EndDay();
                }
            }
        }
    }

    void Awake()
    {

        GameObject go = new GameObject("BiomeContainer");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        biomeContainer = go.transform;

        foreach (Biome b in biomeTemplates)
        {
            b.gameObject.SetActive(false);
        }
        if (regenerate)
        {
            Generate(seed);
        }
    }
    public bool regenerate = false;
    private void Update()
    {
        if (regenerate)
        {
            regenerate = false;
            Generate(seed);
        }
    }

    public void Clear()
    {
        foreach (Transform child in biomeContainer)
            Destroy(child.gameObject);
    }
    public void Generate(int seed)
    {
        Clear();
        Random.InitState(seed);
        startBiome.transform.localPosition = new Vector3(startPosition, 0, 0);
        startBiome.gameObject.SetActive(true);
        endBiome.transform.localPosition = new Vector3(startPosition + biomeSpacing * (numberOfBiome + 1), 0, 0);
        endBiome.gameObject.SetActive(true);

        biomes.Clear();

        biomes.Add(startBiome);
        startBiome.map = this;

        for (int i = 0; i < numberOfBiome; i++)
        {
            var template = biomeTemplates[Random.Range(0, biomeTemplates.Count)];
            Biome b = Instantiate(template);
            b.name = "["+i+"]"+template.name;
            b.map = this;
            biomes.Add(b);
            b.transform.parent = transform;
            b.transform.localPosition = new Vector3(startPosition + biomeSpacing * (i + 1), 0, 0);
            b.transform.localScale = Vector3.one;
            b.transform.localRotation = Quaternion.identity;
            b.transform.parent = biomeContainer;
            b.gameObject.SetActive(true);
        }
        biomes.Add(endBiome);
        endBiome.map = this;
    }
}

