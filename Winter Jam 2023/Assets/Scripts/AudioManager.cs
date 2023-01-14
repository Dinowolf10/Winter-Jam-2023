using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all audio clips that can be played
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        // ensure only one AudioManager exists in a scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // add AudioSource components for all sounds
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Play("MusicLoop");
    }

    /// <summary>
    /// Plays the sound with the provided name
    /// </summary>
    /// <param name="name">name of the sound to be played</param>
    public void Play(string name)
    {
        // find the desired sound in the array
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        sound.source.Play();
    }

    /// <summary>
    /// Plays the sound with the provided name only if it is not currently playing
    /// </summary>
    /// <param name="name">name of the sound to be played</param>
    public void PlayUnique(string name)
    {
        // find the desired sound in the array
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        // if the sound is not currently playing, play it
        if (!sound.source.isPlaying)
        {
            sound.source.Play();
        }
    }

    /// <summary>
    /// Stops the sound with the provided name
    /// </summary>
    /// <param name="name">name of the sound to be stopped</param>
    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        // if the sound is currently playing, stop it
        if (sound.source.isPlaying)
        {
            sound.source.Stop();
        }
    }
}
