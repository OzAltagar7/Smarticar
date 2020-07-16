using UnityEngine;
using System;

/// <summary>
/// An audio manager for handling audio in the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// A <see cref="Sound"/> array consisting of all of the playable sounds.
    /// </summary>
    public Sound[] sounds;

    /// <summary>
    /// Whether or not the audio is currently muted.
    /// </summary>
    public static bool audioIsMuted = false;

    /// <summary>
    /// The audio manager is using the singleton pattern.
    /// This is the audio manager instance.
    /// </summary>
    public static AudioManager instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // If there is already an instance of the audio manager exists
        if (instance == null)
            instance = this;

        // Destroy the new object
        else
        {
            Destroy(gameObject);
            return;
        }

        // Keep the audio manager between scenes
        DontDestroyOnLoad(gameObject);

        // Add the appropriate AudioSource components based on the Sound array
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    /// <summary>
    /// Start is called before the first frame update
    /// Play the main theme at the start of the game.
    /// </summary>
    public void Start()
    {
        Play("MainTheme");
    }

    /// <summary>
    /// Play a given audio
    /// </summary>
    /// <param name="name">The audio to play</param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(string.Format("Sound {0} does not exist", name));
            return;
        }

        s.source.Play();
    }

    /// <summary>
    /// Pause a given audio
    /// </summary>
    /// <param name="name">The audio to pause</param>
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(string.Format("Sound {0} does not exist", name));
            return;
        }

        s.source.Stop();
    }

    /// <summary>
    /// Sets the volume of a given audio
    /// </summary>
    /// <param name="name">The audio to set it's volume</param>
    public void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(string.Format("Sound {0} does not exist", name));
            return;
        }

        s.source.volume = volume;
    }
}
