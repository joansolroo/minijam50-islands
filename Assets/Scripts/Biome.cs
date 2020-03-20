using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cinemachineVirtualCamera.Follow = biomeTarget;
    }
}
