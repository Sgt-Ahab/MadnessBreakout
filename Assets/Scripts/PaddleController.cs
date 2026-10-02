//Name: David Sargent
//Date: 09-29-2026
//Desc: Paddle controller to check collisions, and ball interaction?
//Attach: Paddle
//==========================================
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set RB variable
        paddle_rb = GetComponent<Rigidbody2D>();
        //Get the X value size of the paddle for small paddle collision 
        paddleSize = this.gameObject.transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for powerups, the ball, other things
    }
}
