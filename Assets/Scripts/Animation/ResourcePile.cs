using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePile : MonoBehaviour
{
    private Transform container;

    public SpriteRenderer hands;
    public float spacing;
    public GameObject template;
    public AudioClip clearSound;
    public AudioClip collectSound;
    private AudioSource audiosource;

    public Sprite[] wood;
    public Sprite[] food;
    public Sprite[] water;

    private int stack = 0;
    private List<ResourcePileTemplate> resources = new List<ResourcePileTemplate>();

    void Start()
    {
        GameObject go = new GameObject("ResourceContainer");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;
        container = go.transform;

        audiosource = GetComponent<AudioSource>();
        hands.enabled = false;
    }
    
    public void Add(string type)
    {
        //  select sprite
        Sprite[] s = null;
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
        go.transform.localPosition = new Vector3(0, stack * spacing, stack);
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;

        ResourcePileTemplate res = go.GetComponent<ResourcePileTemplate>();
        res.sr.sprite = s[0];
        res.animations = s;
        go.SetActive(true);
        resources.Add(res);

        stack++;
        audiosource.clip = collectSound;
        audiosource.Play();
        hands.enabled = true;
    }

    public void Clear()
    {
        stack = 0;
        if(resources.Count != 0)
        {
            audiosource.clip = clearSound;
            audiosource.Play();
        }

        foreach (ResourcePileTemplate res in resources)
            res.Destroy();
        resources.Clear();
        hands.enabled = false;
    }
}
