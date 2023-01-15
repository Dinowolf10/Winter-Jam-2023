using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manges the end door's state
/// </summary>
public class Door : MonoBehaviour
{
    private bool isUnlocked;
    [SerializeField]
    private Sprite unlockedDoorSprite;

    // Start is called before the first frame update
    void Start()
    {
        isUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the door's state so that it is accessable by the player
    /// </summary>
    public void UnlockDoor()
    {
        isUnlocked = true;
        GetComponent<SpriteRenderer>().sprite = unlockedDoorSprite;
    }

    /// <summary>
    /// Returns whether or not the door is unlocked
    /// </summary>
    /// <returns>true if the door is unlocked, false otherwise</returns>
    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
