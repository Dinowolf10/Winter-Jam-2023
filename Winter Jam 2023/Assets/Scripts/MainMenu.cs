using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;
    private Player player;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    /// <summary>
    /// Loads the first level of the game
    /// </summary>
    public void PlayGame()
    {
        audioManager.PlayUnique("ButtonClick");
        StartCoroutine(PlayGameCutscene());
    }

    /// <summary>
    /// Exits the game application
    /// </summary>
    public void QuitGame()
    {
        audioManager.PlayUnique("ButtonClick");
        Application.Quit();
    }

    public void ButtonHover()
    {
        audioManager.Play("ButtonHover");
    }

    private IEnumerator PlayGameCutscene()
    {
        Animator playerAnimator = player.GetComponent<Animator>();

        playerAnimator.SetBool("isIdle", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isJumping", true);
        audioManager.PlayUnique("Jump");
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(15.0f, 23.5f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level1");
    }
}
