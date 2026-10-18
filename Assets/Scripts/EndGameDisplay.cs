using TMPro;
using UnityEngine;

public class EndGameDisplay : MonoBehaviour
{
    //This presents the score to the player, and their high score
    public TMP_Text scoreGUI;
    public TMP_Text highScoreGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int score = PlayerPrefs.GetInt("Currentscore", 0);
        scoreGUI.text = "Your Score is: " + score.ToString();
        int highScore = PlayerPrefs.GetInt("Highscore", 0);
        highScoreGUI.text = "Highest Score was: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
