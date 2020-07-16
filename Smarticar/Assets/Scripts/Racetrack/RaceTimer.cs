using UnityEngine;
using System;

/// <summary>
/// A class representing a timer designed specific for the Racetrack mode.
/// The racetrack timer is acting as a stopwatch stopping when the game ends.
/// </summary>
public class RaceTimer : Timer
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        totalTime = 0f;
    }

    /// <summary>
    /// Converts a float type time into a format of mm:ss:ms.
    /// </summary>
    /// <returns>
    /// The time in the desired format.
    /// </returns>
    /// <param name="time">The time to convert.</param>
    public override string ConvertToTimerFormat(float time)
    {
        return TimeSpan.FromMinutes(time).ToString().Substring(0, 8);
    }

    /// <summary>
    /// Returns the total time of the timer in the format of mm:ss:ms.
    /// </summary>
    /// <returns>
    /// The time in the desired format.
    /// </returns>
    public string GetTotalTime()
    {
        return ConvertToTimerFormat(totalTime);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (!paused)
        {
            // Update the timer text with the current total time
            timerText.text = SpaceCharacters(ConvertToTimerFormat(totalTime));
            totalTime += Time.deltaTime;
        }
    }
}
