using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads next level in order of build index, located in build settings.
    }

    public void quitgame()
    {
        Application.Quit();//Quit Game
    }
}
