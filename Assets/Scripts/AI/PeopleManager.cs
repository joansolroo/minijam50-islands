using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    [SerializeField] Transform idleContainer;
    [SerializeField] Transform activeContainer;

    public PlayerController player;
    public List<Follower> idle;
    public List<Follower> agents;
    //private List<int> agentDelay;
    public int actionDelay = 0;

    public int actionQueueSize = 100;
    public float removeCooldown = 0.1f;
    private Vector3 lastPlayerPosition;
    public List<Vector3> playerPositions = new List<Vector3>();
    private float removeCounter = 0f;

    private void Start()
    {
       // agentDelay = new List<int>();
    }
    public void AddFollower()
    {
        if (idle.Count > 0)
        {
            int idx = agents.Count;
            var agent = idle[0];
            
            //agentDelay.Add((int)((idx + 1.5f) * actionDelay));
            agent.gameObject.name = "folower [" + idx/*agentDelay[idx].ToString()*/ + "]";
            agent.speed = player.speed;
            agent.jumpForce = player.jumpForce;
            agent.distToGround = player.distToGround;

            agent.transform.parent = activeContainer;

            agents.Add(agent);
            idle.RemoveAt(0);
        }
    }
    public void RemoveFollower(int idx)
    {
        var agent = agents[idx];
        agent.transform.parent = idleContainer;
        idle.Add(agent);

        agents.RemoveAt(idx);
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
            int index = (int)((i + 1.5f) * actionDelay);// agentDelay[i];
            Follower agent = agents[i];

            if(playerPositions.Count > index)
            {
                agent.target = playerPositions[index];
            }
        }
    }
}
