using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Links")]
    public Level level;
    [Header("Generation")]
    [SerializeField] Transform biomeContainer;
    [SerializeField] public Biome startBiome;
    [SerializeField] public Biome endBiome;
    [SerializeField] List<Biome> biomeTemplates;
    [SerializeField] EnemyController enemyTemplate;
    [Header("Biome Level")]
    [SerializeField] List<BiomeLevelData> biomeLevels;

    public int startPosition = -57;
    public int numberOfBiome = 3;
    public int biomeSpacing;


    [Header("Status")]
    [SerializeField] List<Biome> biomes;
    [SerializeField] int seed;

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

        int numberOfBiomePerDifficultyLevel = numberOfBiome / biomeLevels.Count;

        for (int i = 0; i < numberOfBiome; i++)
        {
            var template = biomeTemplates[Random.Range(0, biomeTemplates.Count)];
            Biome b = Instantiate(template);
            //setting up the value
            b.SetValue(biomeLevels[i / numberOfBiomePerDifficultyLevel].rangeOfResources.Evaluate(Random.value));
            b.name = "["+i+"]"+template.name;
            b.map = this;
            biomes.Add(b);
            b.transform.parent = transform;
            b.transform.localPosition = new Vector3(startPosition + biomeSpacing * (i + 1), 0, 0);
            b.transform.localScale = Vector3.one;
            b.transform.localRotation = Quaternion.identity;
            b.transform.parent = biomeContainer;
            b.gameObject.SetActive(true);

            bool hasEnemy = Random.value < 0.5f;
            if (hasEnemy)
            {
                Vector3 position = enemyTemplate.transform.position;
                position.x = b.biomeTarget.position.x;
                b.enemy = Instantiate<EnemyController>(enemyTemplate, position, Quaternion.identity, b.transform);
                b.enemy.biome = b;
            }
        }
        biomes.Add(endBiome);
        endBiome.map = this;
    }
}

