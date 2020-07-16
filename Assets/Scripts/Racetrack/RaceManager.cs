using System.Collections;
using UnityEngine;

/// <summary>
/// A class for managing the race.
/// </summary>
public class RaceManager : MonoBehaviour
{
    /// <summary>
    /// An array holding a reference for the user and the AI agents.
    /// </summary>
    public CarAgent[] cars;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    public RacetrackUIManager UIManager;

    /// <summary>
    /// The race timer.
    /// </summary>
    public RaceTimer raceTimer;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // Pause the game for the instructions phase
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Starts the game when the user clicks on the start button.
    /// </summary>
    public void OnStartButtonClick()
    {
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// Starts the game by disabling the instructions panel and starting the countdown.
    /// </summary>
    /// <returns>
    /// Waits for the countdown to finish.
    /// </returns>
    IEnumerator StartGame()
    {
        // Hide instructions
        UIManager.instructions.SetActive(false);

        // Start countdown by subtracting from the total time and waits for subtraction to finish
        UIManager.countdownTimer.gameObject.SetActive(true);
        float timeLeftForCountdown = 3f;

        while (timeLeftForCountdown > 0)
        {
            if (timeLeftForCountdown < 1) UIManager.countdownTimer.text = "GO";
            else UIManager.countdownTimer.text = timeLeftForCountdown.ToString("0");

            timeLeftForCountdown -= 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        // Hide all panels
        UIManager.countdownTimer.gameObject.SetActive(false);
        UIManager.darkPanel.SetActive(false);

        // Show timer
        raceTimer.Show(true);

        // Start the game by resetting timeScale to 1f
        Time.timeScale = 1f;

        // the player entered the gameplay phase
        RacetrackUIManager.duringGameplay = true;
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        // Stop agents and timer
        foreach (CarAgent car in cars) car.Stop();
        raceTimer.Pause();

        // Display gameover screen
        StartCoroutine(UIManager.DisplayGameoverScreen(cars[0], cars[1]));
    }
}
