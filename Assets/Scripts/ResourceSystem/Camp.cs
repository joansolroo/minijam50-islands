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

    public Dictionary<string, int> campInventory = new Dictionary<string, int>();

    void Start()
    {
        box = GetComponent<BoxCollider2D>();

        woodText.text = "0/" + woodGoal.ToString();
        foodText.text = "0<color=red>-" + folowerManager.GetFollowerCount().ToString() + "</color>";
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
                foreach (KeyValuePair<string, int> entry in pileItems)
                {
                    if (campInventory.ContainsKey(entry.Key))
                        campInventory[entry.Key] += entry.Value;
                    else campInventory.Add(entry.Key, entry.Value);
                }
                resPile.Clear();
            }

            int woodCount = campInventory.ContainsKey("Wood") ? campInventory["Wood"] : 0;
            int foodCount = campInventory.ContainsKey("Food") ? campInventory["Food"] : 0;

            woodText.text = woodCount.ToString() + "/" + woodGoal.ToString();
            foodText.text = foodCount.ToString() + "<color=red>-" + folowerManager.GetFollowerCount().ToString() + "</color>";
        }
    }

    public void NextDay()
    {
        if (!campInventory.ContainsKey("Food") || campInventory["Food"] <= 0)
            gameOver = true;
        else
        {
            campInventory["Food"] -= folowerManager.GetFollowerCount();
            if (campInventory["Food"] <= 0)
                gameOver = true;
            if (campInventory.ContainsKey("Wood") || campInventory["Wood"] >= woodGoal)
                win = true;
        }
    }
}
