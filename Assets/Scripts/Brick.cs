//Name: David Sargent
//Date: 10-04-2026
//Desc: Brick properties for the GameStateManager to reference
//Attach: BaseBrick prefab
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //Variables
    private int hitsToBreak;
    private int currentHealth;
    private SpriteRenderer colorShifter;
    private bool isPowerUp;
    private float lastHitTime = 0f;
    [SerializeField]
    private GameObject powerUpPrefab;
    [SerializeField]
    private BrickManager BrickConductor;
    //Set the Sprite Renderer Pre-Start with Awake()
    private void Awake()
    {
        colorShifter = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BrickSet();
        ColorSet();
    }
    //HandMades
    private void ColorSet()
    {
        //Set the colors on start to hits to break and currentHealth
        if(currentHealth == hitsToBreak && currentHealth == 3)
        {
            colorShifter.color = new Color32(7, 6, 14, 255);
        }
        else if(currentHealth == hitsToBreak && currentHealth == 2)
        {
            colorShifter.color = new Color32(17, 19, 113, 255);
        }
        else
        {
            colorShifter.color = new Color32(16, 125, 120, 255);
        }
    }
    private void BrickSet()
    {
        //When GameState sets the Bricks, the hitsToBreak is what determines
        //the color of the break in the next step
        if(hitsToBreak == 3)
        {
            currentHealth = 3;
        }
        else if(hitsToBreak == 2)
        {
            currentHealth = 2;
        }
        else if(hitsToBreak == 1)
        {
            currentHealth = 1;
        }
    }
    private void dropPowerUp()
    {
        //Drops powerups at the location of broken brick, checking bool and object
        if (isPowerUp && powerUpPrefab != null)
        {
            //Grab the powerup attached
            Vector3 objectPOS = this.gameObject.transform.position;
            //Takes its spawnPoint, -1 down the Y-axis to get it slightly below it
            Vector2 spawnPoint = new Vector2(objectPOS.x, objectPOS.y - 1);
            //Create the RANDOM powerUpPrefab
            Instantiate(powerUpPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            //If nothing, return
            return;
        }
    }
    private void ColorAdjust()
    {
        //STRICTLY SETS COLOR ON COLLISION AT < 3;
        if(currentHealth == 2)
        {
            colorShifter.color = new Color32(17, 19, 113, 255);
        }
        else if(currentHealth == 1)
        {
            colorShifter.color = new Color32(16, 125, 120, 255);
        }
    }
    public void InitializeBrick(int tier, bool isPower, GameObject powerPrefab)
    {
        //GameStateManager function for direct usage to have its block be called and made into
        //the random algorithm array
        hitsToBreak = tier;
        currentHealth = tier;
        isPowerUp = isPower;
        powerUpPrefab = powerPrefab;

        //Set colorshifter for colors (or sprite sets on health)
        if(colorShifter == null)
        {
            colorShifter = GetComponent<SpriteRenderer>();
        }
        ColorSet();

    }
    public int takeDamage()
    {
        //Dynamically call the brick to return points per hitsToBreak
        //Debounce to breathe between collisions, double colliding was happening
        if(Time.time - lastHitTime < 0.1f)
        {
            return 0;
        }
        //Stamp new time
        lastHitTime = Time.time;

        int points = 0;
        currentHealth--;
        if (currentHealth > 0)
        {
            //Health remains, 1
            ColorAdjust();
            return 0;
        }
        else
        {
            //Ball has broken the block, reward points and drop powerup
            
            switch (hitsToBreak)
            {
                case 3:
                    points = 100;
                    break;
                case 2:
                    points = 50;
                    break;
                case 1:
                    points = 25;
                    break;

            }
            dropPowerUp();
            //Grab the BrickManager's total brick count via FindAnyObjectByType<>.totalBrickCount
            FindAnyObjectByType<BrickManager>().totalBlockCount--;
            Destroy(this.gameObject);
            return points;
        }
    }

}
