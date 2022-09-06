using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Class Sound which allow to add sound in the game
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    // range 0 to 1, volume of the sound
    [Range(0f, 1f)]
    public float volume;

    // range 0.1 to 3, pitch of the sound
    [Range(.1f, 3f)]
    public float pitch;

    // is the sound looping
    public bool loop;

    // add audio source to gameobject, but hide in the inspector
    [HideInInspector]
    public AudioSource source;
}