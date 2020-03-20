using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    private Rigidbody2D body;
    private AnimationController animation;
    private Vector3 direction;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animation = GetComponent<AnimationController>();
        direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(transform.position + Input.GetAxis("Horizontal") * speed * Vector3.right);
        if (Input.GetAxis("Horizontal") == 0f)
            animation.playAnimation(AnimationController.AnimationType.IDLE, direction.x < 0f);
        else
        {
            direction = Input.GetAxis("Horizontal") * Vector3.right;
            animation.playAnimation(AnimationController.AnimationType.WALKING, direction.x < 0f);
        }
    }
}
