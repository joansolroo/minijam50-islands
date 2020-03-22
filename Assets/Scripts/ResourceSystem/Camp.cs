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

    private BoxCollider2D box;
    private RaycastHit2D[] scan = new RaycastHit2D[20];

    public Dictionary<ResourceType, int> campInventory = new Dictionary<ResourceType, int>();

    public int initialFood = 10;
    public Boat boat;
    void Start()
    {
        box = GetComponent<BoxCollider2D>();

        woodText.text = "0/" + woodGoal.ToString();
        foodText.text = initialFood + "<color=red>-" + folowerManager.GetFollowerCount().ToString() + "</color>";
        campInventory[ResourceType.Food] = initialFood;
    }
    
    void Update()
    {
        int hits = Physics2D.BoxCastNonAlloc(box.bounds.center, box.bounds.extents, 0f, Vector2.up, scan, 0.1f, 1 << LayerMask.NameToLayer("ResourcePile"));
        if(hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                ResourcePile resPile = scan[i].collider.gameObject.GetComponent<ResourcePile>();
                var pileItems = resPile.GetResourceList();
                foreach (var type in pileItems.Keys)
                {
                    if(type == ResourceType.People)
                    {
                        for (int p = 0; p < pileItems[type]; ++p) {
                            folowerManager.AddNewAgent(this.transform.position);
                        }
                    }
                    if (campInventory.ContainsKey(type))
                        campInventory[type] += pileItems[type];
                    else campInventory.Add(type, pileItems[type]);
                }
                resPile.Clear();
            }
            woodText.text = GetWood().ToString() + "/" + woodGoal.ToString();
            foodText.text = GetFood().ToString() + "<color=red>-" + folowerManager.GetFollowerCount().ToString() + "</color>";
        }
    }

    public void NextDay()
    {
        if (!campInventory.ContainsKey(ResourceType.Food) || campInventory[ResourceType.Food] <= 0)
            gameOver = true;
        else
        {
           
            campInventory[ResourceType.Food] -= folowerManager.GetFollowerCount();
            if (campInventory[ResourceType.Food] <= 0)
                gameOver = true;
            if (campInventory.ContainsKey(ResourceType.Wood)) {
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
            }
        }
    }
}
