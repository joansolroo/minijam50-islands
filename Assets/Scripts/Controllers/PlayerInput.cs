using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerController player;

    public KeyCode collectResource = KeyCode.Space;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (player.level.camp.gameOver)
        {
            player.SetInput(new PlayerController.InputData
            {
                doJump = false,
                dx =0,
                doInteract = false
            });
        }
        else
        {
            player.SetInput(new PlayerController.InputData
            {
                doJump = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z)),
                dx = Input.GetAxis("Horizontal"),
                doInteract = Input.GetKeyDown(collectResource)
            });
        }
    }
}
