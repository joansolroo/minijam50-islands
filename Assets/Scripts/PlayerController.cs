using UnityEngine;

[RequireComponent(typeof(PlayerResources))]
public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public KeyCode collectResource = KeyCode.Space;

    private Rigidbody2D body;
    private PlayerResources resources;
    private Biome currentBiome;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        resources = GetComponent<PlayerResources>();
    }

    private void Update()
    {
        body.MovePosition(transform.position + Input.GetAxis("Horizontal") * speed * Vector3.right);

        if (currentBiome != null && Input.GetKeyDown(collectResource))
        {
            resources.TryCollect(currentBiome);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentBiome = collision.transform.GetComponent<Biome>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var buildable = collision.transform.GetComponent<Buildable>();
        if (buildable != null)
        {
            var value = resources.Wood.Value;
            buildable.AddWood(value);
            resources.Wood.Remove(value);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var hitBiome = collision.transform.GetComponent<Biome>();
        if (hitBiome != null && hitBiome == currentBiome)
        {
            currentBiome = null;
        }
    }
}
