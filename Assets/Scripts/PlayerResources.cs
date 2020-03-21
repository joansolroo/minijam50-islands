using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IEnumerable<Resource>
{
    public ResourcePile resourcePile;


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
        if (!biome.HasResource)
        {
            return;
        }

        foreach (var resource in this)
        {
            if (biome.ResourceType == resource.GetType())
            {
                float value = biome.Collect();
                resource.Add(value);

                resourcePile.Clear();
                for (int i=0; i<(int)value; i++)
                {
                    resourcePile.Add(resource.GetType().ToString());
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
