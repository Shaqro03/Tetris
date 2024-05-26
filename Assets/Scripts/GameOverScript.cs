using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Canvas classic;
    public Canvas arcade;
    public Text score;
    public Text level;
    public void PlayAgain() 
    {
        if(Board.level != 0) 
        {
            Board.level = 1;
            Board.rowleft = 1;
        }
        Board.numberOfRowsThisTurn = 0;
        Board.currentScore = 0;
        SceneManager.LoadScene("Tetris");
    }

    public void GoToManu()
    {
        Board.numberOfRowsThisTurn = 0;
        Board.currentScore = 0;
        SceneManager.LoadScene("GameMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Start()
    {
        if (Board.level == 0)
        {
            classic.enabled = true;
            score.text = Board.currentScore.ToString();
        }
        else
        {
            arcade.enabled = true;
            level.text = Board.level.ToString();
        }
    }
}
