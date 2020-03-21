using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public ResourcePile resourcePile;
    public PlayerController player;

    [SerializeField]
    private int food;

    [SerializeField]
    private int people;

    [SerializeField]
    private int wood;

    public int Food => food;

    public int People => people;

    public int Wood => wood;

    public bool TryCollect(Biome biome)
    {
        bool success = false;
        if (biome.HasResource)
        {
            int value = (int)biome.Collect();
            switch (biome.ResourceType)
            {
                case ResourceType.None:
                    return false;
                case ResourceType.Food:
                    food += value;
                    break;
                case ResourceType.Wood:
                    wood += value;
                    break;
                case ResourceType.People:
                    people += value;
                    break;
                
                default:
                    break;
            }

            for (int i = 0; i < (int)value; i++)
            {
                player.pickedResources.Add(biome.ResourceType);
            }
            success = true;
        }
        return success;
    }

    public void RemoveWood()
    {
        wood--;
    }
}
