using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [SerializeField] Camp camp;
    [SerializeField] Follower followerTemplate;
    [SerializeField] PlayerAIInput ghostAITemplate;
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
    public void ResetPath()
    {
        playerPositions.Clear();
    }

    public void AddFollower()
    {
        foreach (Follower f in agents)
            f.speed = player.speed;
        foreach (Follower f in idle)
            f.speed = player.speed;

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
    public void FollowerReturnToBase(int idx, List<ResourceType> resourceToCarry)
    {
        var agent = agents[idx];
        agent.transform.parent = idleContainer;
        idle.Add(agent);

        agents.RemoveAt(idx);
        agent.target = boat.position;
        agent.combat = false;

        foreach (var res in resourceToCarry)
            agent.AddResource(res);

        camp.ProcessPile(agent.pile);

        if (agents.Count == 0)
        {
            foreach (Follower f in idle)
                f.speed = 2.5f * player.speed;
        }
    }

    public void AddNewAgent(Vector3 position)
    {
        var newAgent = GameObject.Instantiate<Follower>(followerTemplate);
        newAgent.transform.position = position;
        newAgent.transform.parent = idleContainer;
        idle.Add(newAgent);

        newAgent.target = boat.position;
        newAgent.distToGround = player.distToGround;

        if (agents.Count == 0)
            newAgent.speed = 2f * player.speed;
    }

    public void Retreat()
    {
        foreach (Follower f in idle)
            f.speed = 2.5f * player.speed;
        foreach (Follower f in agents)
            f.speed = 2.5f * player.speed;
    }

    public void EngageCombat(List<Vector3> positions, bool combat = true)
    {
        int i = 0;
        foreach (Follower agent in agents)
        {
            agent.target = positions[i];
            agent.combat = combat;
            i = (i + 1) % positions.Count;
        }
    }

    public int GetIndex(int followerId)
    {
        int spacing = Mathf.Min(actionDelay, (int)(100 / (1 + (float)agents.Count)));
        return Mathf.Clamp((int)((followerId + 2) * spacing), actionDelay, Mathf.Clamp(playerPositions.Count - 1, 5, 99));
    }

    internal bool KillFollower()
    {
        if (idle.Count > 0)
        {
            var agent = idle[0];
            agent.Die();
            AddGhost(agent.transform.position);
            idle.RemoveAt(0);
            return true;
        }
        else if (agents.Count > 0)
        {
            var agent = agents[0];
            AddGhost(agent.transform.position);
            agent.Die();
            agents.RemoveAt(0);
            return true;
        }
        return false;
    }
    public void AddGhost(Vector3 position)
    {
        var ghost = GameObject.Instantiate<PlayerAIInput>(ghostAITemplate);
        ghost.transform.position = position;
        ghost.transform.parent = idleContainer;
    }

    private void Update()
    {
        player.maxStamina = idle.Count + agents.Count;

        removeCounter += 0.8f * Time.deltaTime;

        if ((player.transform.position - lastPlayerPosition).sqrMagnitude > 0.001f)
        {
            playerPositions.Insert(0, player.transform.position);
            if (playerPositions.Count >= actionQueueSize)
                playerPositions.RemoveAt(playerPositions.Count - 1);
            lastPlayerPosition = player.transform.position;
        }
        if (playerPositions.Count > 0 && removeCounter >= removeCooldown)
        {
            playerPositions.RemoveAt(playerPositions.Count - 1);
            removeCounter = 0f;
        }

        if (!player.fighting)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                int index = GetIndex(i);
                Follower agent = agents[i];

                agent.combat = false;
                if (playerPositions.Count > index)
                {
                    agent.target = playerPositions[index];
                }
            }
        }
    }

    public int GetFollowerCount()
    {
        return idle.Count + agents.Count;
    }

    private void OnDrawGizmos()
    {

        if (playerPositions.Count > 1)
        {
            Gizmos.color = Color.green;
            Vector3 previous = playerPositions[0];
            for (int p = 1; p < playerPositions.Count; ++p)
            {
                Vector3 current = playerPositions[p];
                Gizmos.DrawLine(previous, current);
                previous = current;
            }

            Gizmos.color = Color.white;
            for (int i = 0; i < agents.Count; i++)
            {
                int index = GetIndex(i);
                Follower agent = agents[i];
                
                if (playerPositions.Count > index)
                {
                    Vector3 current = playerPositions[index];
                    Gizmos.DrawLine(agent.transform.position, current);
                }
            }
        }
    }
}
