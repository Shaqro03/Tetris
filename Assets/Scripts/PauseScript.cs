using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public void GoToManu()
    {
        Time.timeScale = 1;
        Board.isPaused = false;
        Board.numberOfRowsThisTurn = 0;
        Board.currentScore = 0;
        SceneManager.LoadScene("GameMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
