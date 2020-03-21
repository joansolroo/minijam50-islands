using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public PlayerController player;
    public List<Follower> agents;
    private List<int> agentDelay;
    public int actionDelay = 0;

    public int actionQueueSize = 100;
    public float removeCooldown = 0.1f;
    private Vector3 lastPlayerPosition;
    public List<Vector3> playerPositions = new List<Vector3>();
    private float removeCounter = 0f;

    private void Start()
    {
        agentDelay = new List<int>();
        for (int i = 0; i < agents.Count; i++) 
        {
            agentDelay.Add((int)((i + 1.5f) * actionDelay));
            agents[i].gameObject.name = "folower [" + agentDelay[i].ToString() + "]";
            agents[i].speed = player.speed;
            agents[i].jumpForce = player.jumpForce;
            agents[i].distToGround = player.distToGround;
        }
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
        if(removeCounter >= removeCooldown)
        {
            playerPositions.RemoveAt(playerPositions.Count - 1);
            removeCounter = 0f;
        }
        
        for (int i = 0; i < agents.Count; i++)
        {
            int index = agentDelay[i];
            Follower agent = agents[i];

            if(playerPositions.Count >= index)
            {
                agent.target = playerPositions[index];
            }
        }
    }
}
