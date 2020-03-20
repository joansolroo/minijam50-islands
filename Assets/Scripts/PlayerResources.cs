using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IEnumerable<Resource>
{
    public Food food;

    public Water water;

    public Wood wood;

    public IEnumerator<Resource> GetEnumerator()
    {
        yield return food;
        yield return water;
        yield return wood;
    }

    public void TryCollect(Biome biome)
    {
        if (biome.resource == null)
        {
            return;
        }

        foreach (var resource in this)
        {
            if (biome.resource.GetType() == resource.GetType())
            {
                float value = biome.resource.Value;
                resource.Add(value);
                biome.resource.Remove(value);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
