using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerResources))]
public class PlayerController : MonoBehaviour
{
    [Header("Properties")]
    public int maxStamina = 5;
    private int stamina;
    public bool canMove = true;
    public bool hurt = false;
    public bool fighting = false;
    public bool hasFollowers = true;

    [Header("Links")]
    [SerializeField] Level level;
    [SerializeField] Map map;
    [SerializeField] AnimationController animationController;
    [SerializeField] PlayerResources resources;
    [SerializeField] PeopleManager followers;
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
        public bool doJump;
        public bool doInteract;
    }
    [Header("Status")]
    [SerializeField] Biome previousBiome;
    [SerializeField] Biome currentBiome;
    [SerializeField] Buildable buildable; //TODO better to use an interactable-style for both biomes and buildings alike

    [SerializeField] InputData input;
    [SerializeField] bool jumping = false;
    [SerializeField] Vector2 velocity;
    [SerializeField] Vector3 direction;

    public List<ResourceType> pickedResources = new List<ResourceType>();

    public int Stamina { 
        get => stamina;
        set
        {
            if (hasFollowers)
            {
                if (stamina < value)
                {
                    for (; stamina < value; ++stamina)
                    {
                        followers.AddFollower();
                    }
                }
                else if (stamina > value)
                {
                    for (; stamina > value; --stamina)
                    {
                        followers.RemoveFollower(0, pickedResources);
                        pickedResources.Clear();
                    }
                }
            }
            else
            {
                stamina = value;
            }
        }
    }

    private void Start()
    {
        velocity = Vector2.zero;
        body = GetComponent<Rigidbody2D>();
        resources = GetComponent<PlayerResources>();
        animationController = GetComponent<AnimationController>();
        direction = Vector3.right;
        stamina = 0;
        Rest();
    }

    public void EnterBase()
    {
        hurt = false;
        Stamina = Mathf.Max(1, Stamina);
    }
    public void Rest()
    {
        if (followers)
        {
            Stamina += followers.AvailableCount();
            followers.ResetPath();
        }
        else
        {
            Stamina = maxStamina;
        }
    }

    public void SetInput(InputData _input)
    {
        this.input = _input;
    }
    private void Update()
    {
        if (Stamina <= 0)
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

        if(canMove)
            body.velocity = velocity;

        if (input.dx == 0f)
            animationController.playAnimation(AnimationController.AnimationType.IDLE, direction.x < 0f);
        else
        {
            if (canMove)
                direction = input.dx * Vector3.right;
            animationController.playAnimation(AnimationController.AnimationType.WALKING, direction.x < 0f);
        }
        if (input.doInteract)
        {
            if (buildable)
            {
                TryUpgradeBuildable(buildable);
            }
            if (Stamina > 0 && currentBiome != null && !currentBiome.enemy)
            {
                if (resources.TryCollect(currentBiome))
                {
                    --Stamina;
                    StateChanged();
                }
            }
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
        resources.resourcePile.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var buildable = other.transform.GetComponent<Buildable>();
        if (buildable)
        {
            this.buildable = buildable;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(buildable !=null && collision.gameObject == buildable.gameObject) buildable = null;
    }

    private void TryUpgradeBuildable(Buildable buildable)
    {
        TransferResources();
        Debug.LogWarning("Remove visual item from above character!");
    }

    public void EnterBiome(Biome biome)
    {
        if (currentBiome != biome)
        {
            previousBiome = currentBiome;
            currentBiome = biome;

            level.ChangeBiome(previousBiome, currentBiome);

            if (biome.enemy)
            {
                Fight(biome.enemy);
            }
        }
    }

    public void Fight(EnemyController enemy)
    {
        enemy.Fight(this, followers);
    }
    public void StateChanged()
    {
        level.StateChanged();
    }
}
