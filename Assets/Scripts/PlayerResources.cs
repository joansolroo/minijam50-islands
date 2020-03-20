using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IEnumerable<Resource>
{
    [SerializeField]
    private Food food;

    [SerializeField]
    private Water water;

    [SerializeField]
    private Wood wood;

    public Food Food => food;

    public Water Water => water;

    public Wood Wood => wood;

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
