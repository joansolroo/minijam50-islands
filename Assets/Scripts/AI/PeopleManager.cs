﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [SerializeField] Follower followerTemplate;
    [SerializeField] Transform idleContainer;
    [SerializeField] Transform activeContainer;

    public PlayerController player;
    public Transform boat;
    public List<Follower> idle;
    public List<Follower> agents;
    public int actionDelay = 0;

    public int actionQueueSize = 100;
    public float removeCooldown = 0.1f;
    private Vector3 lastPlayerPosition;
    public List<Vector3> playerPositions = new List<Vector3>();
    private float removeCounter = 0f;

    private void Start()
    {
       
    }
    public bool HasAvailableFollower()
    {
        return idle.Count > 0;
    }
    public int AvailableCount()
    {
        return idle.Count;
    }
    public void AddFollower()
    {
        if (HasAvailableFollower())
        {
            int idx = agents.Count;
            var agent = idle[0];
            
            agent.gameObject.name = "folower [" + idx + "]";
            agent.speed = player.speed;
            agent.jumpForce = player.jumpForce;
            agent.distToGround = player.distToGround;

            agent.transform.parent = activeContainer;

            agents.Add(agent);
            idle.RemoveAt(0);
        }
    }

    public void RemoveFollower(int idx, List<ResourceType> resourceToCarry)
    {
        var agent = agents[idx];
        agent.transform.parent = idleContainer;
        idle.Add(agent);

        agents.RemoveAt(idx);
        agent.target = boat.position;
        agent.goToBase = true;

        foreach (var res in resourceToCarry)
            agent.AddResource(res);

        if(agents.Count == 0)
            agent.speed = 2f * player.speed;
    }

    public void AddNewAgent(Vector3 position)
    {
        var newAgent = GameObject.Instantiate<Follower>(followerTemplate);
        newAgent.transform.position = position;
        newAgent.transform.parent = idleContainer;
        idle.Add(newAgent);

        newAgent.target = boat.position;
        newAgent.goToBase = true;

        if (agents.Count == 0)
            newAgent.speed = 2f * player.speed;
    }
    private void Update()
    {
        removeCounter += 0.8f * Time.deltaTime;

        if ((player.transform.position - lastPlayerPosition).sqrMagnitude > 0.001f)
        {
            playerPositions.Insert(0, player.transform.position);
            if (playerPositions.Count >= actionQueueSize)
                playerPositions.RemoveAt(playerPositions.Count - 1);
            lastPlayerPosition = player.transform.position;
        }
        if(playerPositions.Count>0 && removeCounter >= removeCooldown)
        {
            playerPositions.RemoveAt(playerPositions.Count - 1);
            removeCounter = 0f;
        }
        
        for (int i = 0; i < agents.Count; i++)
        {
            int index = (int)((i + 1.5f) * actionDelay);
            Follower agent = agents[i];

            if(playerPositions.Count > index)
            {
                agent.target = playerPositions[index];
            }
        }
    }
}
