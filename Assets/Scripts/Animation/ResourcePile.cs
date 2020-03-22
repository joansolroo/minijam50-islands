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
    public Sprite[] people;

    private int stack = 0;
    public List<ResourcePileTemplate> resources = new List<ResourcePileTemplate>();

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
    
    public void Add(ResourceType type)
    {
        //  select sprite
        Sprite[] s = null;
        switch (type) {
            case ResourceType.Wood:
                s = wood;
                break;
            case ResourceType.Food:
                s = food;
                break;
            case ResourceType.Water:
                s = water;
                break;
            case ResourceType.People:
                s = people;
                break;
            default:
                s = wood;
                Debug.LogError("Unknown resource type : " + type);
                break;
        }

        //  instantiate
        GameObject go = Instantiate(template);
        go.name = type.ToString();
        go.transform.parent = container;
        go.transform.localPosition = new Vector3(0, stack * spacing, stack);
        go.transform.localScale = Vector3.one;
        go.transform.localRotation = Quaternion.identity;

        ResourcePileTemplate res = go.GetComponent<ResourcePileTemplate>();
        res.type = type;
        res.sr.sprite = s[0];
        res.animations = s;
        go.SetActive(true);
        resources.Add(res);

        stack++;
        audiosource.clip = collectSound;
        audiosource.Play();
        hands.enabled = true;
    }

    public void ClearData()
    {
        if (resources.Count != 0)
        {
            audiosource.clip = clearSound;
            audiosource.Play();
        }
    }
    public void ClearVisuals()
    {
        stack = 0;
        foreach (ResourcePileTemplate res in resources)
            res.Destroy();
        resources.Clear();
        hands.enabled = false;
    }

    public Dictionary<ResourceType, int> GetResourceList()
    {
        Dictionary<ResourceType, int> result = new Dictionary<ResourceType, int>();
        foreach(ResourcePileTemplate res in resources)
        {
            if (result.ContainsKey(res.type))
                result[res.type]++;
            else result.Add(res.type, 1);
        }
        return result;
    }
}
