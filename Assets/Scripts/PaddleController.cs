//Name: David Sargent
//Date: 09-29-2026
//Desc: Paddle controller to check collisions, and ball interaction?
//Attach: Paddle
//==========================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//Paddle needs tto change state for the PowerUp, give them IDs?
//List Variables: ShuffleSpeed, BallForce
public class PaddleController : MonoBehaviour
{
    //Serialize/Publics
    private Rigidbody2D paddle_rb;
    [SerializeField]
    private float ShuffleSpeed;
    [SerializeField]
    private float BallForce;
    [SerializeField]
    private Vector3 paddleSize;
    private bool isSmall;
    private float SmallTimer = 0f;
    public InputAction paddleMove;
    //Two bools for checking wall side
    private bool touchLeftWall;
    private bool touchRightWall;
    public GameObject ballPrefab;
    private GameStateManager GameBrain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the GameStateManager to root powerups and clones under it's transform
        GameBrain = FindAnyObjectByType<GameStateManager>();
        //Set RB variable
        paddle_rb = GetComponent<Rigidbody2D>();
        //Get the X value size of the paddle for small paddle collision 
        paddleSize = transform.localScale;
        paddleMove.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        movePaddle();
        if (isSmall)
        {
            SmallTimer -= Time.deltaTime;
            if(SmallTimer <= 0f)
            {
                //Return back to normal size
                isSmall = false;
                transform.localScale = paddleSize;
            }
        }
    }
    //Helpers
    private void movePaddle()
    {
        //Adjust for position to be post checks and stop the paddle.
        Vector2 move = paddleMove.ReadValue<Vector2>();
        //Check if we are touching the wall, and inputting
        if(touchLeftWall && move.x < 0)
        {
            move.x = 0;
        }
        //Using CPP with dual ifs 
        if(touchRightWall && move.x > 0)
        {
            move.x = 0;
        }
        Vector2 position = (Vector2)transform.position + move * ShuffleSpeed * Time.deltaTime;

        transform.position = position;
    }
    //Prebuilts
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for powerups, the ball, other things
        if(collision.gameObject.CompareTag("OB"))
        {
            //If the paddle touches the OB-Barrier, then it needs to not go that direction
            if(collision.contacts[0].normal.x > 0.5)
            {
                touchLeftWall = true;
            }
            else if (collision.contacts[0].normal.x < -0.5)
            {
                touchRightWall = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OB"))
        {
            touchLeftWall = false;
            touchRightWall= false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check what powerup hits the paddle
        PowerUp collidingPower = collision.GetComponent<PowerUp>();
        if (collidingPower != null)
        {
            //Double collisions, turn off the collider
            collision.enabled = false;
            switch (collidingPower.type)
            {
                case PowerUp.PowerUpType.ExtraLife:
                    //Object Name is ExtraLife
                    GameBrain.AddLife();
                    break;
                case PowerUp.PowerUpType.ThreeBalls:
                    //3Ball sends the balls upwards from where grabbed
                    Vector2 spawnPOS = collision.transform.position;
                    for(int i = 0; i < 3; i++)
                    {
                        float xOffset = (1 * i) + 1;
                        GameObject ballClone = Instantiate(ballPrefab, spawnPOS, Quaternion.identity, GameBrain.transform);
                        ballClone.GetComponent<BallController>().isClone = true;
                        Rigidbody2D ballCloneRB = ballClone.GetComponent<Rigidbody2D>();
                        ballCloneRB.linearVelocity = new Vector2(xOffset, 5f);
                        ballCloneRB.bodyType = RigidbodyType2D.Dynamic;
                        
                    }
                    break;
                case PowerUp.PowerUpType.SmallPaddle:
                    isSmall = true;
                    SmallTimer = 10f;
                    transform.localScale = new Vector3(paddleSize.x * 0.75f, paddleSize.y, paddleSize.z);
                    break;


            }
            Destroy(collision.gameObject);
        }
    }
}
