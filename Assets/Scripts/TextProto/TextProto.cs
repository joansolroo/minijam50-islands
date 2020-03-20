using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextProto : MonoBehaviour
{
    public static int water = 2;
    public static int food = 2;
    public static int wood = 0;

    public static bool hasResource = false;

    public static bool isAlive = true;
    public static Direction dir = Direction.Right;
    public static int currentNode = 0;
    public static int nextNode = 1;
    public static Node[] level = new Node[7];
    public static System.Random rand = new System.Random();
    public static ArrayList poolOfTiles = new ArrayList();

    public static int boatStatus = 0;
    public static int boatFinished = 100;

    public static int cabinStatus = 0;
    public static int cabinFinished = 100;

    //input management
    public static bool isWaitingForInput = false;
    public static bool gotInput = false;

    public enum Type
    {
        Forest,
        River,
        Food,
        Cabin,
        Boat,
    };

    public enum Direction
    {
        Right,
        Left
    };

    public struct Node
    {
        public string name;
        public string description;
        public Type type;
        public int nbResources;

        public Node(
            string _name,
            string _description,
            Type _t,
            int _nb)
        {
            name = _name;
            description = _description;
            type = _t;
            nbResources = _nb;
        }
    };

    // Start is called before the first frame update
    void Start()
    {
        Node cabin = new Node("       CABANE", "Ici, tu dors, puis le jour se lève, et c’est reparti!", Type.Cabin, 0);
        Node boat = new Node("       PLAGE", "Ici tu peux construire ton bateau pour fuire", Type.Boat, 0);

        //Create biome 
        //Forest
        for (int i = 1; i < 5; i++)
        {
            poolOfTiles.Add(new Node("       FORET", "Tu est dans une forêt, veux tu prendre du bois ?", Type.Forest, i));
        }

        //River
        for (int i = 1; i < 5; i++)
        {
            poolOfTiles.Add(new Node("       RIVIERE", "Tu est au bord d’une rivière, veux tu prendre de l’eau ?", Type.River, i));
        }

        //Food
        for (int i = 1; i < 5; i++)
        {
            poolOfTiles.Add(new Node("       BUISSON", "Il y a plein de buissons avec des fruits, veux tu prendre de la nourriture ?", Type.Food, i));
        }

        level[0] = cabin;
        level[6] = boat;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            Debug.Log("GAME OVER");
            return;
        }

        if (isWaitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentNode = nextNode;
                if (dir == Direction.Right)
                {
                    nextNode++;
                }
                else
                {
                    nextNode--;
                }

                if (nextNode < 0)
                {
                    nextNode = 1;
                    dir = Direction.Right;
                }

                if (nextNode > 6)
                {
                    nextNode = 5;
                    dir = Direction.Left;
                }

                isWaitingForInput = false;
            }
            else if (currentNode % 6 != 0 && !hasResource && Input.GetKeyDown(KeyCode.UpArrow))
            {
                switch (level[currentNode].type)
                {
                    case Type.Forest:
                        wood += level[currentNode].nbResources;
                        break;
                    case Type.River:
                        water += level[currentNode].nbResources;
                        break;
                    case Type.Food:
                        food += level[currentNode].nbResources;
                        break;
                    default:
                        break;
                }
                hasResource = true;
                isWaitingForInput = false;
            }
        }
        else
        {
            string message = "";
            if (currentNode == 6)
            {
                boatStatus += wood;
                wood = 0;
                message += "BOAT : " + boatStatus + "/" + boatFinished + "\n";
                hasResource = false;
            }
            else if (currentNode == 0)
            {
                water--;
                food--;
                FillLevel();

                cabinStatus += wood;
                wood = 0;

                if (water < 0 || food < 0)
                {
                    isAlive = false;
                }
                message += "CABIN : " + cabinStatus + "/" + cabinFinished + "\n";
            }
            else
            {
                message += level[currentNode].name + "(RES:" + level[currentNode].nbResources + ")\n";
            }
            
            message += "EAU: " + water + " // FOOD: " + food + " // BOIS: " + wood;

            //Console.WriteLine(level[currentNode].description);

            message += " //// NEXT : " + level[nextNode].type;
            message += " //// You can : ";
            message += "MOVE";
            if (currentNode % 6 != 0 && !hasResource)
            {
                message += " OR GATHER RESOURCES (" + level[currentNode].nbResources + ")\n";
            }

            Debug.Log(message);
            isWaitingForInput = true;
        }
    }

    public static void Reset()
    {
        isAlive = true;
        dir = Direction.Right;
        currentNode = 0;
    }

    public static void FillLevel()
    {
        for (int i = 1; i < 6; i++)
        {
            level[i] = (Node)poolOfTiles[rand.Next(poolOfTiles.Count)];
        }
    }
}
