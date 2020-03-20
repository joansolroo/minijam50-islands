using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public float jumpForce = 10f;
    public float fallingFriction = 0.05f;
    public bool jumping = false;
    public float distToGround;

    private Vector2 velocity;
    private Rigidbody2D body;
    private AnimationController animationController;
    private Vector3 direction;

    private void Start()
    {
        velocity = Vector2.zero;
        body = GetComponent<Rigidbody2D>();
        animationController = GetComponent<AnimationController>();
        direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
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
}
