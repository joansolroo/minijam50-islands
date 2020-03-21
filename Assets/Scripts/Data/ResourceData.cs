using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ResourceData", menuName = "Data/Resource", order = 2)]
public class ResourceData : ScriptableObject
{
    public enum ResourceType
    {
        Wood, Food, Water
    }
    public ResourceType type;
}
