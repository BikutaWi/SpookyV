using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound Manager class, allow to play sound in the game
/// </summary>
public class SoundManager : MonoBehaviour
{
    // Array of sounds
    public Sound[] sounds;

    public static SoundManager instance;

    /// <summary>
    /// When game start
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // for all sound in the array
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    /// <summary>
    /// When the game start (after Awake methods), play background music
    /// </summary>
    private void Start()
    {
        // play background music
        Play("background");
    }

    /// <summary>
    /// Methods to play a sound in the game
    /// </summary>
    /// <param name="name">name of the sound</param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(name));
        if(s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found...");
            return;
        }

        s.source.Play();
        
    }

    /// <summary>
    /// Methods to stop a sound in the game
    /// </summary>
    /// <param name="name">name of the sound</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(name));
        if(s == null)
        {
            Debug.LogWarning("Sound : " + name + " not found...");
            return;
        }

        s.source.Stop();
    }
}
