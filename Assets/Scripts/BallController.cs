//Name: David Sargent
//Date: 10-03-2026
//Desc: This is the Ball Controller script
//Attach: Ball(Game Item)
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallController : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float ballSpeed = 3;
    private float maxBallSpeed = 12;
    private Vector2 velocity;
    public bool ballEnabled = true;
    public bool isClone = false;
    //Store the Ball of reference for recalling/recreating
    public GameObject ball;
    //Spawn Location
    public GameObject ballSpawnPOS;
    public Vector2 spawnPoint;
    public InputAction balldrop;
    private bool isBallDrop = false;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = ballSpawnPOS.transform.position;
        balldrop.Enable();
        this.gameObject.transform.position = spawnPoint;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        //Velocity is set to (0,0)
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        dropBall();
    }
    //Self-Mades
    private void incrementSpeed()
    {
        //Every contact WITH PADDLE (place on collision) increments + 1
        if(ballSpeed < maxBallSpeed)
        {
            ballSpeed = ballSpeed + 0.25F;
        }
        else
        {
            return;
        }
    }
    private void dropBall()
    {
        //Drop the ball from input, and flip the bool;
        if(!isBallDrop)
        {
                if(balldrop.IsPressed() && rb.bodyType == RigidbodyType2D.Static)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;            
                    velocity = new Vector2(0f, -1f) * ballSpeed;
                    rb.linearVelocity = velocity;
                    isBallDrop = false;
                    ballSpeed = 3;
                }
        }
    }
    //PreBuilts
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("End"))
        {
            //the GameManager can keep count of balls, and if balls are 0, deduct a life
            if(ballEnabled && isClone)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.transform.position = spawnPoint;
                rb.bodyType = RigidbodyType2D.Static;
                isBallDrop = false;
                ballSpeed = 3;
                //Reduce a life as well
            }
        }
        //Paddle Logic
        else if(collision.gameObject.CompareTag("Paddle"))
        {
            incrementSpeed();
            //Gather what the paddle data size is
            float paddleCenter = collision.transform.position.x;
            //Use its collider for full width
            float paddleWidth = collision.collider.bounds.size.x;
            //Calculate the X value for offset
            float offset = (this.gameObject.transform.position.x - collision.transform.position.x) / (paddleWidth / 2);
            //Assemble the newAngle for balls redirection
            Vector2 newAngle = new Vector2(offset, 1.0f).normalized;
            //Normalize the data with .normalized    
            //New Velocity is our Vector2 * ballSpeed
            rb.linearVelocity = newAngle * ballSpeed;
        }
    }
}
