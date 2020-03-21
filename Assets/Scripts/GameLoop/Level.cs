﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] DayTime time;
    [Header("Properties")]
    [SerializeField] int seed;
    [SerializeField] Map map;
    [SerializeField] int[] daySeeds;

    [Header("Status")]
    [SerializeField] int currentDay = 0;

    public void Initialize()
    {
        Random.InitState(seed);
        daySeeds = new int[30];
        for (int i = 0; i < 30; ++i)
        {
            daySeeds[i] = Random.Range(0, 65000);

        }
    }
    public void StartLevel()
    {
        map.level = this;
        map.Generate(currentDay) ;
    }
    public void StartDay()
    {
        time.GoToNextMorning();
    }
    public void EndDay()
    {
        ++currentDay;
        time.GoToNight();
        map.Generate(currentDay);
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
