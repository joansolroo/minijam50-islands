using UnityEngine;

public class Biome : MonoBehaviour
{
    public Transform biomeTarget;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;

    public Resource resource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cinemachineVirtualCamera.Follow = biomeTarget;
    }
}
