using UnityEngine;

/// <summary>
/// A class representing an audio file 
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// The name of the track
    /// </summary>
    public string name;

    /// <summary>
    /// The AudioClip of the track
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// The volume of the track (goes between 0 and 1).
    /// </summary>
    [Range(0f, 1f)]
    public float volume;

    /// <summary>
    /// The pitch of the track (goes between 0.1 and 3).
    /// </summary>
    [Range(0.1f, 3f)]
    public float pitch;

    /// <summary>
    /// Whether or not the track should be looped.
    /// </summary>
    public bool loop;

    /// <summary>
    /// The AudioSource component of the track.
    /// </summary>
    [HideInInspector]
    public AudioSource source;
    
}
