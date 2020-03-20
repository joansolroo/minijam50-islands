using UnityEngine;

[RequireComponent(typeof(PlayerResources))]
public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public KeyCode collectResource = KeyCode.Space;
    public float jumpForce = 10f;
    public float fallingFriction = 0.05f;
    public bool jumping = false;
    public float distToGround;

    private Vector2 velocity;
    private Rigidbody2D body;
    private PlayerResources resources;
    private Biome currentBiome;
    private AnimationController animationController;
    private Vector3 direction;

    private void Start()
    {
        velocity = Vector2.zero;
        body = GetComponent<Rigidbody2D>();
        resources = GetComponent<PlayerResources>();
        animationController = GetComponent<AnimationController>();
        direction = Vector3.right;
    }

    private void Update()
    {
        jumping = !IsGrounded();
        if (!jumping && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z)))
        {
            body.AddForce(new Vector2(0f, jumpForce));
            jumping = true;
        }
        if(body.velocity == Vector2.zero && jumping)
        {
            transform.position -= fallingFriction * Vector3.up;
        }

        velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
        Vector3 dummy = Vector3.zero;
        body.velocity = velocity;

        if (Input.GetAxis("Horizontal") == 0f)
            animationController.playAnimation(AnimationController.AnimationType.IDLE, direction.x < 0f);
        else
        {
            direction = Input.GetAxis("Horizontal") * Vector3.right;
            animationController.playAnimation(AnimationController.AnimationType.WALKING, direction.x < 0f);
        }
        if (currentBiome != null && Input.GetKeyDown(collectResource))
        {
            resources.TryCollect(currentBiome);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, distToGround, 1 << LayerMask.NameToLayer("Ground"));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded() ? Color.red : Color.white;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * distToGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnterBiome(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryUpgradeBuildable(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ExitBiome(collision);
    }

    private void TryUpgradeBuildable(Collider2D collision)
    {
        var buildable = collision.transform.GetComponent<Buildable>();
        if (buildable != null)
        {
            var value = resources.Wood.Value;
            buildable.Upgrade(value);
            resources.Wood.Remove(value);
        }
    }

    private void EnterBiome(Collision2D collision)
    {
        currentBiome = collision.transform.GetComponent<Biome>();
    }

    private void ExitBiome(Collision2D collision)
    {
        var hitBiome = collision.transform.GetComponent<Biome>();
        if (hitBiome != null && hitBiome == currentBiome)
        {
            currentBiome = null;
        }
    }
}
