using TMPro;
using UnityEngine;

/// <summary>
/// Timer that determines how much time the player has to complete a level
/// </summary>
public class Timer : MonoBehaviour
{
    // total amount of time provided for the level
    [SerializeField]
    private float startTime;

    // time at which the player is given a warning
    [SerializeField]
    private float warningTimeMarker;

    // amount of time remaining
    private float runningTime;

    // determines if time is counting down
    private bool isTicking;

    // UI object that displays the time to the player
    [SerializeField]
    private TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        runningTime = startTime;
        isTicking = true;
    }

    // Update is called once per frame
    void Update()
    {
        // decrement the timer and update the UI text
        if (isTicking)
        {
            runningTime -= Time.deltaTime;
            timerText.text = runningTime.ToString("00.0");
        }

        // if remaining time is low, update text color to warn player
        if (runningTime <= warningTimeMarker)
        {
            timerText.color = Color.red;
        }
        
        // if remaining time is up, go to end game state
        if (runningTime <= 0.0f)
        {
            ExpireTime();
        }
    }

    /// <summary>
    /// Handles the event when the player runs out of time in a level
    /// </summary>
    void ExpireTime()
    {
        // TODO: IMPLEMENT LOSE CONDITION
        timerText.text = "Time's Up!";
    }

    /// <summary>
    /// Stops the timer from counting down
    /// </summary>
    public void StopTicking()
    {
        isTicking = false;
    }
}
