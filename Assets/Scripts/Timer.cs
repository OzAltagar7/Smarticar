using UnityEngine;
using TMPro;

/// <summary>
/// The basic timer class.
/// This is the base for both the racetrack timer and the parking timer.
/// </summary>
public abstract class Timer : MonoBehaviour
{
    /// <summary>
    /// The timer text displaying the total time of the timer.
    /// </summary>
    public TextMeshProUGUI timerText;

    /// <summary>
    /// the timer panel.
    /// </summary>
    public GameObject timerPanel;

    /// <summary>
    /// the total time of the timer.
    /// </summary>
    public float totalTime;

    /// <summary>
    /// whether or not the timer is paused.
    /// </summary>
    protected bool paused;

    /// <summary>
    /// Pause the timer.
    /// </summary>
    public void Pause()
    {
        paused = true; 
    }

    /// <summary>
    /// Show/Hide the timer based on the given activation status.
    /// </summary>
    /// <param name="activationStatus">Whether on not to show the timer.</param>
    public void Show(bool activationStatus)
    {
        timerPanel.SetActive(activationStatus);
        timerText.gameObject.SetActive(activationStatus);
    }

    /// <summary>
    /// Insert a space between each character of the timer text. 
    /// </summary>
    /// <returns>
    /// The spaced timer.
    /// </returns>
    /// <param name="time">The timer text to space.</param>
    protected string SpaceCharacters(string time)
    {
        string spacedTimer = "";

        for (int i = 0; i < time.Length; i++)
        {
            if (i != time.Length - 1) spacedTimer += time[i] + " ";
            else spacedTimer += time[i];
        }

        return spacedTimer;
    }

    /// <summary>
    /// Convert the timer text into the desired format.
    /// For the racetrack mode- mm:ss:ms.
    /// For the parking mode- ss:ms.
    /// </summary>
    /// <returns>
    /// The time in the desired form.
    /// </returns>
    /// <param name="time">The time to convert.</param>
    public abstract string ConvertToTimerFormat(float time);
}
