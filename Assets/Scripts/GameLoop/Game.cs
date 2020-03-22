using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] Level levelInstance;
    [SerializeField] GameObject coverScreen;
    [SerializeField] GameObject mainMenu;

    [SerializeField] AudioSource music;
    public enum GameStatus
    {
        Uninitialized, Cover, Start, Menu, Play, Paused , Over
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
        if(status == GameStatus.Over)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartLevel();
            }
        }
    }
    public void StartLevel()
    {
        music.pitch = 0.75f;
        GenerateLevel();
        coverScreen.SetActive(false);
    }

    public void RestartLevel()
    {
        StartLevel();
    }
    public void GenerateLevel()
    {
        var previous = levelInstance;
        levelInstance = GameObject.Instantiate<Level>(level);
        levelInstance.gameObject.SetActive(true);
        if (previous)
        {
            GameObject.Destroy(previous.gameObject);
        }
        levelInstance.gameObject.SetActive(true);
        status = GameStatus.Play;
        levelInstance.Initialize();
        levelInstance.StartLevel();
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public void OnLose()
    {
        Debug.Log("Lose");
        music.pitch = 0.4f;
        StartCoroutine(WaitForEnd(2));
    }
    public void OnWin()
    {
        music.pitch = 1f;
        StartCoroutine(WaitForEnd(3));
    }

    IEnumerator WaitForEnd(float time)
    {
        yield return new WaitForSeconds(time);
        status = GameStatus.Over;
    }
}
