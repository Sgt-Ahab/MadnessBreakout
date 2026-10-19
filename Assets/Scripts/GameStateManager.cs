//Name: David Sargent
//Date: 10-01-2026
//Desc: Game Manager, manages balls, ball vectors, and paddle locations possibly
//Attach: GameManager(Empty Parent)
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//My thought is to make this the state manager, check what block the ball hits, update score, hierarchy the blocks[BrickState manages, just reference the score],
//Possibly even make the random layouts be from here? Or make that a parent and a block layout<Split in two layers via BrickManager.cs>
//Make an algorithm that takes a space, and occupies it space, make prefabs to infer for usage? <Done, BrickManager.cs>
//Make L Key clear Level to showcase next Level generation
public class GameStateManager : MonoBehaviour
{
    private int maxLives = 3;
    private int levelCount = 0;
    public TMP_Text guiScore;
    public TMP_Text levelLabel;
    public TMP_Text livesLabel;
    private BrickManager brickManager;
    private PlayerScore playerScore;
    private EnvironmentManager envManager;
    private bool allowLevelClear = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brickManager = GetComponentInChildren<BrickManager>();
        playerScore = GetComponent<PlayerScore>();
        envManager = GetComponent<EnvironmentManager>();
        incrementLevel();
        showLevel();
        showLife();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowLevelClear && Input.GetKeyDown(KeyCode.L))
        {
            //Take all active bricks in the scene and destroy them
            Brick[] allBrick = FindObjectsByType<Brick>();
            foreach (Brick b in allBrick)
            {
                
                Destroy(b.gameObject);
            }
            //Force the LevelBuild Threshold to trigger
            brickManager.totalBlockCount = 0;
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Bunker");
            foreach(GameObject sp in spawns)
            {
                Destroy(sp);
            }
        }
        if (maxLives > 0)
        {
            LevelBuildCheck();
        }
        else
        {
            gameOver();
        }
    }
    
    private void incrementLevel()
    {
        //Make it so each call level Goes up, internal via logic checks
        levelCount++;
    }
    public void showLevel()
    {
        levelLabel.text = "Level: " + levelCount.ToString();
    }    
    public void showScore()
    {
        //High score
        if (playerScore.getScore() > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", playerScore.getScore());
            Debug.Log("New Highscore saved");
        }
        guiScore.text = "Final Score: " + playerScore.getScore().ToString();
    }
    public void AddLife()
    {
        maxLives++;
        livesLabel.text = "x " + maxLives.ToString();
        
    }
    public void TakeLife()
    {
        maxLives--;
        livesLabel.text = "x " + maxLives.ToString();
    }
    public void gameOver()
    {
        //Run to make "Highscore" have an integer, else getScore for the final;
        PlayerPrefs.SetInt("Currentscore", playerScore.getScore());
        showScore();

        Time.timeScale = 1f;
        SceneManager.LoadScene("EndGame");

    }
    public void resetGame()
    {
        maxLives = 3;
        levelCount = 1;
    }
    public void showLife()
    {
        livesLabel.text = "x " + maxLives.ToString();
    }
    public void LevelBuildCheck()
    {
        //Strictly for Checking the GameState
        if(!brickManager.BlocksRemain())
        {
            //First, reset ball
            BallController[] BallArray = this.GetComponentsInChildren<BallController>();
            //Iterate the array and prep next level for each
            foreach(BallController ball in BallArray)
            {
                ball.PrepNextLevel();
            }
            //Next, create new Level
            brickManager.AssembleNewLevel();
            incrementLevel();
            envManager.AssembleEnvironment();
            showLevel();
        }
    }
}
