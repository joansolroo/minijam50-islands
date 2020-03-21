using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] DayTime time;
    [SerializeField] PlayerController player;
    [SerializeField] Boat boat;
    [SerializeField] List<CharacterController> characters;

    [Header("HUD")]
    [SerializeField] GameObject morningCover;
    [SerializeField] GameObject nightCover;
    [SerializeField] GameObject tiredCover;
    [SerializeField] GameObject hurtCover;
    [Header("Properties")]
    [SerializeField] int seed;
    [SerializeField] int maxDays = 30;
    [SerializeField] Map map;
    [SerializeField] int[] daySeeds;

    [Header("Status")]
    [SerializeField] int currentDay = 0;

    public void Initialize()
    {
        Random.InitState(seed);
        daySeeds = new int[maxDays];
        for (int i = 0; i < maxDays; ++i)
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
        Debug.Log("Start day "+ currentDay);
        time.GoToNextMorning();
        player.Rest();
        morningCover.SetActive(true);
    }

    public void StateChanged()
    {
        tiredCover.SetActive(player.stamina <= 0);
    }
    public void ChangeBiome(Biome previousBiome, Biome currentBiome)
    {
        Debug.Log("Biome:" + previousBiome + "->" + currentBiome);

        morningCover.SetActive(false);
        nightCover.SetActive(false);

        if (previousBiome != null && currentBiome == map.startBiome)
        {
            player.stamina = Mathf.Max(1, player.stamina);
            this.EndDay();
        }
        else if (previousBiome == map.startBiome)
        {
            this.StartDay();
        }
        StateChanged();
    }
    public void EndDay()
    {
        nightCover.SetActive(true);
        Debug.Log("End of day "+ currentDay);
        ++currentDay;
        time.GoToNight();
        map.Generate(currentDay);
        Debug.LogWarning("Do this when interacting with the proper object instead");
        player.TransferResources();
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
