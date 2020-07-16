using UnityEngine;
using System;

/// <summary>
/// A class representing a timer designed specific for the Parking mode.
/// The parking timer is acting as a countdown timer starts at 60 seconds.
/// </summary>
public class ParkingTimer : Timer
{
    /// <summary>
    /// The game manager of the game
    /// </summary>
    public Manager gameManager;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Reset the clock at 60 seconds
        totalTime = 60f;
    }

    /// <summary>
    /// Convert a float type time into a format of ss:ms.
    /// </summary>
    /// <returns>
    /// The time in the desired format.
    /// </returns>
    /// <param name="timePassed">The time to convert.</param>
    public override string ConvertToTimerFormat(float timePassed)
    {
        return SpaceCharacters(TimeSpan.FromMinutes(timePassed).ToString().Substring(3, 5));
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Update the timer text with the current total time
        timerText.text = ConvertToTimerFormat(totalTime);
        if (totalTime - Time.deltaTime >= 0) totalTime -= Time.deltaTime;

        // Making sure the timer doesn't go below 0
        else
        {
            totalTime = 0;
            Time.timeScale = 0f;
            gameManager.EndGame();
        }
    }
}
