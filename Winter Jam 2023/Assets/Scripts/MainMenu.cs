using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Loads the first level of the game
    /// </summary>
    public void PlayGame()
    {
        audioManager.PlayUnique("ButtonClick");
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Exits the game application
    /// </summary>
    public void QuitGame()
    {
        audioManager.PlayUnique("ButtonClick");
        Application.Quit();
    }

    public void ButtonHoverSound()
    {
        audioManager.Play("ButtonHover");
    }
}
