using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public bool gameOver = false;
    public bool win = false;
    public int woodGoal = 10;

    public TextMesh woodText;
    public TextMesh foodText;
    public PeopleManager folowerManager;
    public Transform followerSpawn;
    private BoxCollider2D box;
    private RaycastHit2D[] scan = new RaycastHit2D[20];

    public Dictionary<ResourceType, int> campInventory = new Dictionary<ResourceType, int>();

    public int initialFood = 10;
    public Boat boat;
    public Color colorTextNegative;
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        campInventory[ResourceType.Food] = initialFood;

        UpdateGUI();
    }

    void Update()
    {
        int hits = Physics2D.BoxCastNonAlloc(box.bounds.center, box.bounds.extents, 0f, Vector2.up, scan, 0.1f, 1 << LayerMask.NameToLayer("ResourcePile"));
        if (hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                ResourcePile resPile = scan[i].collider.gameObject.GetComponent<ResourcePile>();
                resPile.ClearVisuals();
            }
            UpdateGUI();
        }
    }
    public void ProcessPile(ResourcePile resPile)
    {
        var pileItems = resPile.GetResourceList();
        foreach (var type in pileItems.Keys)
        {
            if (type == ResourceType.People)
            {
                for (int p = 0; p < pileItems[type]; ++p)
                {
                    folowerManager.AddNewAgent(followerSpawn.position);
                }
            }
            if (campInventory.ContainsKey(type))
                campInventory[type] += pileItems[type];
            else campInventory.Add(type, pileItems[type]);
        }
        resPile.ClearData();
    }
    void UpdateGUI()
    {
        woodText.text = GetWood().ToString() + "/" + woodGoal.ToString();
        foodText.text = GetFood().ToString() + "<color=#"+ColorUtility.ToHtmlStringRGB(colorTextNegative )+ ">-" + folowerManager.GetFollowerCount().ToString() + "</color>";
    }
    public void NextDay()
    {
        //if (!campInventory.ContainsKey(ResourceType.Food) || campInventory[ResourceType.Food] <= 0)
        //    gameOver = true;
        //else
        {

            int followers = folowerManager.GetFollowerCount();
            int food = campInventory[ResourceType.Food];
            if (food >= followers)
            {
                campInventory[ResourceType.Food] = food - followers;
            }
            else
            {
                campInventory[ResourceType.Food] = 0;

                int deaths = followers - food;
                if (deaths > followers) deaths = followers;

                for (int f = 0; f < deaths; ++f)
                {
                    folowerManager.KillFollower();
                    --followers;
                }
                if (followers == 0)
                {
                    gameOver = true;
                }
            }



            if (campInventory.ContainsKey(ResourceType.Wood))
            {
                boat.Progress = Mathf.Clamp01(campInventory[ResourceType.Wood] / (float)woodGoal);
                if (campInventory[ResourceType.Wood] >= woodGoal)
                {
                    win = true;
                }
            }
        }
    }

    public int GetFood()
    {
        return campInventory.ContainsKey(ResourceType.Food) ? campInventory[ResourceType.Food] : 0;
    }
    public int GetWood()
    {
        return campInventory.ContainsKey(ResourceType.Wood) ? campInventory[ResourceType.Wood] : 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                player.EnterBase();
                UpdateGUI();
            }

        }
    }
}
