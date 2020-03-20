using UnityEngine;

public class Biome : MonoBehaviour
{
    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    public Resource resource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cinemachineVirtualCamera.Follow = biomeTarget;
    }
}
