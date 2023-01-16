using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    private AudioManager audioManager;

    private void Start()
    {
        slider.value = 1;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void AdjustVolume()
    {
        AudioListener.volume = slider.value;
    }

    public void ButtonHover()
    {
        audioManager.Play("ButtonHover");
    }
}
