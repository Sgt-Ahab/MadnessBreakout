//Name: David Sargent
//Date: 10-05-2026
//Desc: Counts bricks, places bricks within gameGrid (topLeft - bottomRight)
//      Spawning prefabs in Layer1, then filling with Bricks in Layer 2 for true random level generation
//Attach: BlockManager

//Use .transform.childCount for gathering prefab counts
//KISS it out by using three prefab points, then grid beneath
//Have the repeats be a separate list, remove when using so levels truly progress, then once clear recycle the list
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BrickManager : MonoBehaviour
{
    //Variable inits + Headers [Header("HeaderName")] for Brick's visibility
    //Make the Grid size
    [Header("Grid Size")]
    [SerializeField]
    private Transform topLeft;
    [SerializeField]
    private Transform bottomRight;
    private Vector2 gridSize;
    
    //Object References
    [Header("Block Pieces")]
    [SerializeField]
    //Multiple, using an array
    private GameObject[] brickBlockObjects;
    //Prefab Spawns
    [SerializeField]
    private GameObject prefabSpawnParent;
    [SerializeField]
    private GameObject baseBrick;
    [SerializeField]
    private GameObject[] powerUpObjects;
    //repeatLedger is a list to make is dynamic size
    private List<GameObject> repeatLedger;
    //This is spawnPoints for layer1
    private Transform[] spawnPoints;

    //Grid settings
    [Header("Grid Settings")]
    [SerializeField]
    //On Awake, make the brickSize to baseBrick x / y
    private Vector2 brickSize = new Vector2();
    [SerializeField]
    //I am thinking using an 1/8th width of brick.Y
    private Vector2 brickPadding = new Vector2();
    //This parses the bricks, for the GameManager to view
    public int totalBlockCount;
    //Awake gets before start, so we can gather our brickSize, and brickPadding
    private void Awake()
    {
        repeatLedger = new List<GameObject>();
        ListRepopulation();
        // Check if baseBrick is not null, then get size from bounds.size
        if (baseBrick != null)
        {
            SpriteRenderer bb = baseBrick.GetComponent<SpriteRenderer>();
            if(bb != null)
            {
                brickSize = bb.bounds.size;
            }
        }
        //Set padding as base X, and 1/8 y;
        brickPadding = new Vector2(0.1f, brickSize.y / 8f);

        //Gather the child spawn points
        int spawnCount = prefabSpawnParent.transform.childCount;
        spawnPoints = new Transform[spawnCount];
        for(int i = 0; i < spawnCount; i++)
        {
            spawnPoints[i] = prefabSpawnParent.transform.GetChild(i);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBricks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Helpers
    private void PopulateLayer1()
    {
        //Check if 0, return
        if (brickBlockObjects.Length == 0)
        {
            return;
        }
        
        //repopulate the list
        ListRepopulation();

        //Pick one prefab to populate
        int objectChoice = Random.Range(0, repeatLedger.Count);
        GameObject selectedObject = repeatLedger[objectChoice];

        //Remove the selected object
        repeatLedger.RemoveAt(objectChoice);

        //Make each spawn point have the item
        foreach (Transform point in spawnPoints)
        {
            GameObject item = Instantiate(selectedObject, point.position, Quaternion.identity, transform);

            //Count the bricks from each item
            totalBlockCount += item.transform.childCount;

        } 
    }
    private void SpawnBricks()
    {
        //Fill Layer 1. then Layer 2
        PopulateLayer1();

        //Begin Layer 2
        //Layer 2 is where remaining base bricks go, start with grid size float points + the brick size
        float xMin = topLeft.position.x + (brickSize.x / 2f);
        float xMax = bottomRight.position.x - (brickSize.x / 2f);
        float yMax = topLeft.position.y;
        float yMin = bottomRight.position.y;

        float yMid = yMax - ((yMax - yMin) / 2f);
        //CLOCK-IN POINT: FINISH THE LAYER TWO SO THE GRID SHOWS,
        //Get random cluster prefab from array
        int randomIndex = Random.Range(0, brickBlockObjects.Length);
        GameObject randomFab = brickBlockObjects[randomIndex];

        //Calculate spawn positions
    }
    private void ListRepopulation()
    {
        //Check for existence, and size
        if(repeatLedger == null)
        {
            repeatLedger = new List<GameObject>();
        }
        //Once the repeated list == 0, rerun and regather the prefabs
        if(repeatLedger.Count == 0)
        {
            for (int i = 0; i < brickBlockObjects.Length; i++)
            {
                repeatLedger.Add(brickBlockObjects[i]);
            }
        }
    }
    private void InitializeAllBricks()
    {
        //This is what goes and randomizes and sets them with powerups
    }
    //Prebuilts
}
