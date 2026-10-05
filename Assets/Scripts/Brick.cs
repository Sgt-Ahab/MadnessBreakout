//Name: David Sargent
//Date: 10-04-2026
//Desc: Brick properties for the GameStateManager to reference
//Attach: BaseBrick prefab
using Unity.Mathematics;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //Variables
    private int hitsToBreak;
    private int currentHealth;
    private int points;
    private SpriteRenderer colorShifter;
    private bool isPowerUp;
    [SerializeField]
    private GameObject powerUpPrefab;
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
            points = 50;
        }
        else if(hitsToBreak == 2)
        {
            currentHealth = 2;
            points = 25;
        }
        else if(hitsToBreak == 1)
        {
            currentHealth = 1;
            points = 5;
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
        //STRICTLY SETS COLOR ON COLLISION SO < 3;
        if(currentHealth == 2)
        {
            colorShifter.color = new Color32(17, 19, 113, 255);
        }
        else if(currentHealth == 1)
        {
            colorShifter.color = new Color32(16, 125, 120, 255);
        }
    }
    public void InitializeBrick(int health, int pointValue, bool isPower, GameObject powerPrefab)
    {
        //GameStateManager function for direct usage to have its block be called and made into
        //the random algorithm array
        hitsToBreak = health;
        currentHealth = health;
        points = pointValue;
        isPowerUp = isPower;
        powerUpPrefab = powerPrefab;

        //Set colorshifter for colors (or sprite sets on health)
        if(colorShifter == null)
        {
            colorShifter = GetComponent<SpriteRenderer>();
        }
        ColorSet();

    }
    //Prefabs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            currentHealth--;
            if(currentHealth >= 1)
            {
                //Health remains, 1+
                ColorAdjust();
            }
            else
            {
                //Ball has broken the block, reward points and drop powerup
                dropPowerUp();
                //GameStateManager.Instance.AddScore()
                Destroy(this.gameObject);
            }
        }
    }
}
