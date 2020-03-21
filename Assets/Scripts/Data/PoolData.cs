using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolData", menuName = "Data/Pool", order = 1)]
public class PoolData : ScriptableObject
{
    public BiomeData[] biomes;
}