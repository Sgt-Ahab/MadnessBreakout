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
    private float paddleSize;
    public InputAction paddleMove;
    //Two bools for checking wall side
    private bool touchLeftWall;
    private bool touchRightWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set RB variable
        paddle_rb = GetComponent<Rigidbody2D>();
        //Get the X value size of the paddle for small paddle collision 
        paddleSize = this.gameObject.transform.localScale.x;
        paddleMove.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        movePaddle();
    }
    //Helpers
    private void movePaddle()
    {
        Vector2 move = paddleMove.ReadValue<Vector2>();
        Vector2 position = (Vector2)transform.position + move * ShuffleSpeed * Time.deltaTime;
        //Check if we are touching the wall, and inputting
        if(touchLeftWall && move.x < 0)
        {
            move.x = 0;
        }
        //Using CPP with dual ifs 
        if(touchRightWall && move.y < 0)
        {
            move.y = 0;
        }    
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
}
