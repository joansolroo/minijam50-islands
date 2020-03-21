using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCamp : MonoBehaviour
{
    [SerializeField] PeopleManager people;
    List<ResourceType> resources = new List<ResourceType>();

    public static FireCamp main;

    private void Awake()
    {
        main = this;
    }
    internal void Get(ResourcePile pile)
    {
        foreach (var resource in pile.resources)
        {
            if(resource.type == ResourceType.People)
            {
                people.AddNewAgent(this.transform.position);
            }
            else
            {
                resources.Add(resource.type);
            }
        }
        pile.Clear();
    }
}
