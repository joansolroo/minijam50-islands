using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public string name;
    public BiomeData[] biomes;
}
