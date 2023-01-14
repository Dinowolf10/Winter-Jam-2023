using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks the collectables acquired by the player and updates the respective UI elements
/// </summary>
public class CollectableManager : MonoBehaviour
{
    // number of collectables required to complete a level
    [SerializeField]
    private int requiredCollectabeles;

    // number of collectables the player currently has acquired
    private int acquiredCollectables;

    // UI prefab 
    [SerializeField]
    private Image collectableIconPrefab;

    // UI elements displaying 
    private List<Image> collectableIcons;

    // amount of space between each collectable UI icon
    [SerializeField]
    private int collectableIconOffset;

    // UI canvas
    [SerializeField]
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        collectableIcons = new List<Image>();
        acquiredCollectables = 0;

        int totalOffset = 0;
        for (int i = 0; i < requiredCollectabeles; i++)
        {
            collectableIcons.Add(Instantiate(collectableIconPrefab));
            collectableIcons[i].transform.SetParent(canvas.transform);
            collectableIcons[i].rectTransform.anchoredPosition = new Vector2(50 + totalOffset, -50);
            collectableIcons[i].color = new Color(1.0f, 1.0f, 1.0f, 0.25f);
            totalOffset += collectableIconOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Updates game and UI when a player acquires a collectable
    /// </summary>
    public void UpdateCollectables()
    {
        if (acquiredCollectables < requiredCollectabeles)
        {
            collectableIcons[acquiredCollectables].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            acquiredCollectables++;
        }

        if (acquiredCollectables == requiredCollectabeles)
        {
            // unlock door
        }
    }
}
