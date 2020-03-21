using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{
    private Transform container;

    public float spacing;
    public GameObject template;
    public Sprite wood;
    public Sprite food;
    public Sprite water;
    public Rigidbody2D playerBody;

    private int stack = 0;

    void Start()
    {
        GameObject go = new GameObject("ResourceContainer");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        container = go.transform;
    }
    
    public void Add(string type)
    {
        //  select sprite
        Sprite s = null;
        if (type == "Wood")
            s = wood;
        else if(type == "Food")
            s = food;
        else if (type == "Water")
            s = water;
        else
        {
            s = wood;
            Debug.LogError("Unknown resource type : " + type);
        }

        //  instantiate
        GameObject go = Instantiate(template);
        go.name = type;
        go.transform.parent = container;
        go.transform.localPosition = new Vector3(0, 0.85f + stack * spacing, stack);
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;

        go.GetComponent<SpriteRenderer>().sprite = s;
        go.SetActive(true);

        stack++;
    }

    public void Clear()
    {
        stack = 0;
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
