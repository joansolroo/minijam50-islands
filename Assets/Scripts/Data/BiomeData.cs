using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "Data/Biome", order = 2)]
public class BiomeData : ScriptableObject
{
    public string name;
    [Range(1, 5)] public int dificulty = 1;
    [System.Serializable]
    public struct ResourceEntry
    {
        public ResourceData resource;
        public AnimationCurve quantity;
    }
    public ResourceEntry[] resources;
    
}
