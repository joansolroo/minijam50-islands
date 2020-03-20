using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Biome startBiome;
    public Biome endBiome;
    public List<Biome> biomeList;
    public Transform biomeContainer;

    public int startPosition = -57;
    public int numberOfBiome = 3;
    public int biomeSpacing;

    void Start()
    {
        GameObject go = new GameObject("BiomeContainer");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        biomeContainer = go.transform;

        Initialize();
    }

    public void Initialize()
    {
        foreach (Transform child in biomeContainer)
            Destroy(child.gameObject);

        startBiome.transform.localPosition = new Vector3(startPosition, 0, 0);
        startBiome.gameObject.SetActive(true);
        endBiome.transform.localPosition = new Vector3(startPosition + biomeSpacing * (numberOfBiome+1), 0, 0);
        endBiome.gameObject.SetActive(true);

        for (int i = 0; i < numberOfBiome; i++)
        {
            Biome b = Instantiate(biomeList[Random.Range(0, biomeList.Count)]);
            b.transform.parent = transform;
            b.transform.localPosition = new Vector3(startPosition + biomeSpacing * (i + 1), 0, 0);
            b.transform.localScale = Vector3.one;
            b.transform.localRotation = Quaternion.identity;
            b.transform.parent = biomeContainer;
            b.gameObject.SetActive(true);
        }
    }
}
