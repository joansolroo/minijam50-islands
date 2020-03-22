using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeLevelData", menuName = "Data/BiomeLevel", order = 1)]
public class BiomeLevelData : ScriptableObject
{
    public AnimationCurve rangeOfResources;
    public AnimationCurve rangeOfNMI;
}
