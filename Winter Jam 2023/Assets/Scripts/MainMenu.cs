using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the first level of the game
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Exits the game application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
