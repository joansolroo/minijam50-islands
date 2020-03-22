using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] DayTime time;
    [SerializeField] PlayerController player;
    [SerializeField] Boat boat;
    public Camp camp;
    [SerializeField] List<CharacterController> characters;

    [Header("HUD")]
    [SerializeField] GameObject morningCover;
    [SerializeField] GameObject nightCover;
    [SerializeField] GameObject tiredCover;
    [SerializeField] GameObject fightCover;
    [SerializeField] GameObject hurtCover;
    [SerializeField] GameObject winCover;
    [SerializeField] GameObject loseCover;
    [Header("Properties")]
    [SerializeField] int seed;
    [SerializeField] int maxDays = 30;
    [SerializeField] Map map;
    [SerializeField] int[] daySeeds;

    [Header("Status")]
    [SerializeField] int currentDay = 0;

    [Header("Audio")]
    private AudioSource audiosource;
    public AudioClip endDaySound;
    public AudioClip startDaySound;

    [Header("End")]
    public BoatEnd boatEnding;
    public GameObject islandLimit;

    public void Initialize()
    {
        Random.InitState(seed);
        daySeeds = new int[maxDays];
        for (int i = 0; i < maxDays; ++i)
        {
            daySeeds[i] = Random.Range(0, 65000);

        }
        audiosource = GetComponent<AudioSource>();
    }
    public void StartLevel()
    {
        map.level = this;
        map.Generate(currentDay);
    }
    public void StartDay()
    {
        time.GoToNextMorning();
        morningCover.SetActive(true);

        audiosource.clip = startDaySound;
        audiosource.Play();
    }

    public void StateChanged()
    {
        if (camp.win)
        {
            winCover.SetActive(true);
        }
        if (camp.gameOver)
        {
            loseCover.SetActive(true);
        }

        fightCover.SetActive(player.fighting);
        hurtCover.SetActive(player.hurt);
        tiredCover.SetActive(!player.hurt && player.Stamina <= 0);
    }
    public void ChangeBiome(Biome previousBiome, Biome currentBiome)
    {
        morningCover.SetActive(false);
        nightCover.SetActive(false);

        if(previousBiome == null)
        {
            // first entry
        }
        else if(previousBiome == map.startBiome)
        {
            this.StartDay();
        }
        else if(previousBiome.position > currentBiome.position)
        {
            player.DoRetreat();
        }
        
        StateChanged();
    }
    public void EndDay()
    {
        nightCover.SetActive(true);
        ++currentDay;
        time.GoToNight();
        map.Generate(currentDay);
        camp.NextDay();
        player.Rest();

        audiosource.clip = endDaySound;
        audiosource.Play();

        if (camp.win)
            OnWin();
        else if (camp.gameOver)
            OnLose();
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
        camp.boat.gameObject.SetActive(false);
        boatEnding.transform.parent = null;
        boatEnding.gameObject.SetActive(true);
        for (int i = 0; i < boatEnding.slots.Count; i++)
            boatEnding.slots[i] += boatEnding.transform.position;
        camp.folowerManager.EngageCombat(boatEnding.slots, false);
        player.GetComponent<PlayerInput>().enabled = false;
        player.SetInput(new PlayerController.InputData { doJump = false, dx = -1, doInteract = false });
        StartCoroutine(PlayerMovementEnding());
        camp.folowerManager.enabled = false;
        islandLimit.SetActive(false);

        StartCoroutine(EndCoregraphy());
    }




    private IEnumerator PlayerMovementEnding()
    {
        while(player.transform.position.x > boatEnding.transform.position.x + boatEnding.captainSlot.x)
        {
            player.SetInput(new PlayerController.InputData { doJump = false, dx = -1, doInteract = false });
            camp.folowerManager.ResetPath();
            yield return new WaitForFixedUpdate();
        }
        player.SetInput(new PlayerController.InputData { doJump = false, dx = 0, doInteract = false });
    }
    private IEnumerator EndCoregraphy()
    {
        yield return new WaitForSeconds(4f);

        camp.folowerManager.enabled = false;
        foreach(Follower agent in camp.folowerManager.agents)
        {
            agent.transform.parent = boatEnding.transform;
            agent.canMove = false;
        }
        player.transform.parent = boatEnding.transform;
        boatEnding.speed = -1f;
        yield return new WaitForSeconds(8f);
    }
}
