using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cinemachineVirtualCamera.Follow = biomeTarget;
    }
}
