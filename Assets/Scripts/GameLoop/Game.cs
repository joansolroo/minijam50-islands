using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Level level;
  
    public void Initialize()
    {
        Debug.Log("Begin");
    }

    private void Start()
    {
        StartLevel();
    }
    public void StartLevel()
    {
        level.Initialize();
        level.StartDay();
    }

    public void RestartLevel()
    {
        Debug.Log("Restart");
    }

    public void QuitLevel()
    {
        Debug.Log("Quit");
    }

    public void OnLose()
    {
        Debug.Log("Win");
    }
    public void OnWin()
    {
        Debug.Log("Lose");
    }
}
