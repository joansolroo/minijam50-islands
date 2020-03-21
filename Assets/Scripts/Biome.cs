using System;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public Map map;
    public BiomeData data;

    public string activationTag = "Player";
    public EnemyController enemy;

    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    public List<GameObject> toEnable = new List<GameObject>();
    public List<GameObject> toDisable = new List<GameObject>();

    [SerializeField]
    private Resource resource;

    public bool HasResource => resource != null;

    public Type ResourceType => resource.GetType();

    public float Collect()
    {
        foreach (GameObject go in toEnable)
            go.SetActive(true);
        foreach (GameObject go in toDisable)
            go.SetActive(false);

        float value = resource.Value;
        resource.Remove(value);
        return value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == activationTag)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                player.EnterBiome(this);
                cinemachineVirtualCamera.Follow = biomeTarget;
            }
        }
    }
}
