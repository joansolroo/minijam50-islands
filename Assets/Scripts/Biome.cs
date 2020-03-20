﻿using System;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField]
    private Resource resource;

    public bool HasResource => resource != null;

    public Type ResourceType => resource.GetType();

    public float Collect()
    {
        float value = resource.Value;
        resource.Remove(value);
        return value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cinemachineVirtualCamera.Follow = biomeTarget;
    }
}
