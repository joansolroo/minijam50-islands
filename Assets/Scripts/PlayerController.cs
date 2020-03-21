using UnityEngine;

[RequireComponent(typeof(PlayerResources))]
public class PlayerController : MonoBehaviour
{
    [Header("Properties")]
    public int maxStamina = 5;
    public int stamina;

    [Header("Links")]
    [SerializeField] Level level;
    [SerializeField] Map map;
    [SerializeField] AnimationController animationController;
    [SerializeField] PlayerResources resources;

    [Header("Controller")]
    private Rigidbody2D body;
    public float speed = 0.1f;
    public float jumpForce = 10f;
    public float fallingFriction = 0.05f;
    public float distToGround;


    [System.Serializable]
    public struct InputData
    {
        public float dx;
        public bool doJump ;
        public bool doInteract;
    }
    [Header("Status")]
    [SerializeField] Biome previousBiome;
    [SerializeField] Biome currentBiome;

    [SerializeField] InputData input;
    [SerializeField] bool jumping = false;
    [SerializeField] Vector2 velocity;
    [SerializeField] Vector3 direction;

    private void Start()
    {
        velocity = Vector2.zero;
        body = GetComponent<Rigidbody2D>();
        resources = GetComponent<PlayerResources>();
        animationController = GetComponent<AnimationController>();
        direction = Vector3.right;
        Rest();
    }
    
    public void Rest()
    {
        stamina = maxStamina;
    }

    public void SetInput(InputData _input)
    {
        this.input = _input;
    }
    private void Update()
    {
        if (stamina <= 0)
        {
            input.dx = -2;
        }
        jumping = !IsGrounded();
        if (!jumping && input.doJump)
        {
            body.AddForce(new Vector2(0f, jumpForce));
            jumping = true;
        }
        if (body.velocity == Vector2.zero && jumping)
        {
            transform.position -= fallingFriction * Vector3.up;
        }

        velocity = new Vector2(input.dx * speed, body.velocity.y);
        Vector3 dummy = Vector3.zero;
        body.velocity = velocity;

        if (input.dx == 0f)
            animationController.playAnimation(AnimationController.AnimationType.IDLE, direction.x < 0f);
        else
        {
            direction = input.dx * Vector3.right;
            animationController.playAnimation(AnimationController.AnimationType.WALKING, direction.x < 0f);
        }
        if (stamina>0 && currentBiome != null && input.doInteract)
        {
            resources.TryCollect(currentBiome);
            --stamina;
            level.StateChanged();
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


    public void TransferResources()
    {
        Debug.LogWarning("Implement the transfer of resources to the main stack.");
        foreach (var resource in resources)
        {
            resource.Remove(resource.Value);
        }
        resources.resourcePile.Clear();
    }
    /* Deprecated for now:
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryUpgradeBuildable(other);
    }

    private void TryUpgradeBuildable(Collider2D other)
    {
        var buildable = other.transform.GetComponent<Buildable>();
        if (buildable != null)
        {
            var value = resources.Wood.Value;
            buildable.Upgrade(value);
            resources.Wood.Remove(value);
        }
    }*/

    public void EnterBiome(Biome biome)
    {
        if (currentBiome != biome)
        {
            previousBiome = currentBiome;
            currentBiome = biome;

            level.ChangeBiome(previousBiome, currentBiome);
        }
    }

}
