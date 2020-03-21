using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Level level;

    [SerializeField] GameObject coverScreen;
    [SerializeField] GameObject mainMenu;
    public enum GameStatus
    {
        Uninitialized, Cover, Start, Menu, Play, Paused
    }
    GameStatus status = GameStatus.Uninitialized;
    public void Initialize()
    {
        Debug.Log("Begin");
        level.gameObject.SetActive(false);
    }
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        Cover();
    }
    public void Cover()
    {
        coverScreen.SetActive(true);
        status = GameStatus.Cover;
    }
    private void Update()
    {
        if (status == GameStatus.Cover)
        {
            if (Input.anyKeyDown)
            {
                StartLevel();
            }
        }
    }
    public void StartLevel()
    {
        level.gameObject.SetActive(true);
        status = GameStatus.Play;
        level.Initialize();
        level.StartLevel();
        coverScreen.SetActive(false);
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
        Debug.Log("Lose");
    }
    public void OnWin()
    {
        Debug.Log("Win");
    }
}
