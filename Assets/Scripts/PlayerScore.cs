using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    //Variable inits, for score
    private int score;
    public TMP_Text guiScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
    }
    public int getScore()
    {
        return score;
    }    
    public void incrementScore(int val)
    {
        score += val;
        guiScoreUpdate();
    }
    public void guiScoreUpdate()
    {
        guiScore.text = "Score: ";
        guiScore.text += score.ToString();
    }
}
