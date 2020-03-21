using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Data/Biome", order = 2)]
public class BiomeData : ScriptableObject
{
    public BiomeLevelData biomeLevel;
    public ResourceType type;
}
