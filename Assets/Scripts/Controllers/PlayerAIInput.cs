using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIInput : MonoBehaviour
{
    [SerializeField] PlayerController player;
    float velocity;
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        bool jump = Random.value < 0.001f;
        if (Random.value < 0.01f)
        {   if (Random.value < 0.5f)
            {
                velocity = Random.Range(-1, 2) / 2f;
            }
            else
            {
                velocity = 0;
            }
        }

        player.SetInput(new PlayerController.InputData
        {
            doJump = jump,
            dx = velocity,
            doInteract = false
        });

    }
}
