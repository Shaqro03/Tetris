using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManuScript : MonoBehaviour
{
    public void PlayClassic() 
    {
        Board.level = 0;
        Board.rowleft = 0;
        SceneManager.LoadScene("Tetris");
    }


    public void PlayArcade()
    {
        Board.level = 1;
        Board.rowleft = 1;
        SceneManager.LoadScene("Tetris");
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
