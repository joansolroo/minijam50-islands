using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum ResourceType
{
    Wood, Food, People, Water, None
}

[CreateAssetMenu(fileName = "ResourceData", menuName = "Data/Resource", order = 2)]
public class ResourceData : ScriptableObject
{
    public ResourceType type;
}
