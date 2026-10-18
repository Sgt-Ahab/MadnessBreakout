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
    private Transform paddleTransform;
    public InputAction balldrop;
    private bool isBallDrop = false;
    //Time buffer to prevent double-call
    private bool isDead = false;
    private Rigidbody2D rb;
    private PlayerScore player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Ball Clones receiving this as well, bool check clone state, make else dynamic
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerScore>();

        //Main-Ball Only snaps to paddle
        if(!isClone)
        {
            paddleTransform = GameObject.FindGameObjectWithTag("Paddle").transform;
            balldrop.Enable();
            rb.bodyType = RigidbodyType2D.Static;
            //Velocity is set to (0,0)
            velocity = Vector2.zero;
        }
        else
        {
            //Release the Clones!
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.bodyType == RigidbodyType2D.Static)
        {
            this.gameObject.transform.position = new Vector2(paddleTransform.position.x, paddleTransform.position.y + 1.5f);
        }
        dropBall();
        PreventStall();
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
                //Check if pressed, and body type
                if(balldrop.IsPressed() && rb.bodyType == RigidbodyType2D.Static)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    isDead = false;
                    velocity = new Vector2(0f, -1f) * ballSpeed;
                    rb.linearVelocity = velocity;
                    isBallDrop = false;
                    ballSpeed = 3;
                }
        }
    }
    private void PreventStall()
    {
        //Helper Function to prevent the ball stall
        if(rb.bodyType == RigidbodyType2D.Dynamic)
        {
            //Check if less than 0.2f
            if(Mathf.Abs(rb.linearVelocity.y) < 0.2f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1f);
            }
        }
    }
    public void PrepNextLevel()
    {
        //Helper function to reset the ball
        if(isClone)
        {
            //No new balls in the next level
            Destroy(this.gameObject);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            isBallDrop = false;
            //Leave the ballspeed, good luck players
        }
    }
    //PreBuilts
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("End"))
        {
            //Time maybe askewed from moving the object, just turn off collider
            if(isDead)
            {
                return;
            }

            //the GameManager can keep count of balls, and if balls are 0, deduct a life
            if(ballEnabled && isClone)
            {
                ballEnabled = false;
                Destroy(this.gameObject);
            }
            else
            {
                isDead = true;
                //Reduce a life as well
                FindAnyObjectByType<GameStateManager>().TakeLife();
                //Use the bodyType to snap the spawn back above the paddle
                rb.bodyType = RigidbodyType2D.Static;
                isBallDrop = false;
                ballSpeed = 3;
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
        //Add negative Y bias to ball to prevent horizontal ball-stalls
        else if(collision.gameObject.CompareTag("Brick"))
        {
            Brick hitBrick = collision.gameObject.GetComponent<Brick>();
            if(hitBrick != null)
            {
                //Debug.Log("Brick is found and points added");
                player.incrementScore(hitBrick.takeDamage());
            }
            //Inverting the Y caused X to stay the same, thus consuming the bricks...
            //Vector2 inverseSpeed = rb.linearVelocity;
            //float inverseY = inverseSpeed.y * -1;
            //inverseSpeed.y = inverseY;
            //rb.linearVelocity = inverseSpeed;
        }
    }
}
