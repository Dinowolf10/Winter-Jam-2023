using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void LoadLevel(int number)
    {
        SceneManager.LoadScene("Level" + number.ToString());
    }

    public void ButtonHover()
    {
        audioManager.Play("ButtonHover");
    }
}
