using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private FollowPlayer camFollow;
    [SerializeField]
    private Vector2 endWallSpeed;
    [SerializeField]
    private Canvas pauseMenu;
    public bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        cam = Camera.main;
        camFollow = cam.GetComponent<FollowPlayer>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Loads the next level after x amount of seconds
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadLevelTransition()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        audioManager.StopAllEnvironmentSounds();
    }

    /// <summary>
    /// Out of time death "cutscene"
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoseLevelTransition()
    {
        camFollow.playerDead = true;
        while (camFollow.leftWallEnd.position.x < cam.transform.position.x || camFollow.rightWallEnd.position.x > cam.transform.position.x)
        {
            if (camFollow.leftWallEnd.position.x >= cam.transform.position.x)
            {
                camFollow.leftWallEnd.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
            }

            if (camFollow.rightWallEnd.position.x <= cam.transform.position.x)
            {
                camFollow.rightWallEnd.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
            }

            audioManager.PlayUnique("Boulder");
            camFollow.leftWallEnd.Translate(endWallSpeed * Time.fixedDeltaTime);
            camFollow.rightWallEnd.Translate(-endWallSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        player.StartCoroutine(player.HandleDeath());
    }

    public void OnPause(InputValue value)
    {
        if (player.inCutscene)
        {
            return;
        }

        if (gamePaused)
        {
            pauseMenu.gameObject.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1;
            player.GetComponent<PlayerInput>().enabled = true;
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            gamePaused = true;
            Time.timeScale = 0;
            player.GetComponent<PlayerInput>().enabled = false;
        }
    }
}
