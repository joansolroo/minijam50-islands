using UnityEngine;

[RequireComponent(typeof(PlayerResources))]
public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public KeyCode collectResource;

    private Rigidbody2D body;
    private PlayerResources resources;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        resources = GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(transform.position + Input.GetAxis("Horizontal") * speed * Vector3.right);

        if (Input.GetKey(collectResource))
        {
            // TODO
        }
    }
}
