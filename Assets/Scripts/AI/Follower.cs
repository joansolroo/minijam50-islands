﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float verticalVelocity;
    private Vector3 lastPosition;
    private Vector3 direction;
    public Vector3 target;
    private AnimationController animationController;
    public ResourcePile pile;

    public bool jumping = false;
    public float distToGround;
    public bool goToBase = false;

    FireCamp fireCamp;

    void Start()
    {
        verticalVelocity = 0;
        animationController = GetComponent<AnimationController>();
        target = transform.position;
        animationController.timeIdle += Random.Range(-0.03f, 0.03f);
        fireCamp = FireCamp.main;
    }
    
    void Update()
    {
        Vector3 delta = (target - transform.position).normalized * speed * Time.deltaTime;
        if ((target - transform.position).sqrMagnitude < 0.001f)
            delta = Vector3.zero;

        jumping = !IsGrounded();
        if (jumping)
            verticalVelocity -= 9.81f * Time.deltaTime;
        else
            verticalVelocity = 0f;
        if (delta.y > 0.01f && !jumping)
        {
            jumping = true;
            verticalVelocity = jumpForce * Time.deltaTime;
        }
        
        
        delta = new Vector3(delta.x, verticalVelocity * Time.deltaTime, 0);
        transform.position += delta;

        if (lastPosition != transform.position)
        {
            direction = transform.position - lastPosition;
            direction.Normalize();
        }

        if (delta.sqrMagnitude < 0.000001f)
        {
            animationController.playAnimation(AnimationController.AnimationType.IDLE, direction.x < 0);
            if(goToBase)
            {
                goToBase = false;
                fireCamp.Get(pile);
            }
        }
        else
            animationController.playAnimation(AnimationController.AnimationType.WALKING, direction.x < 0);

        lastPosition = transform.position;
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, distToGround, 1 << LayerMask.NameToLayer("Ground"));
    }
    public void AddResource(ResourceType type)
    {
        pile.Add(type);
    }
}
