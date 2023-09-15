using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGame : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("GAME");//game scene
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Main Menu");//main menu
    }
}
