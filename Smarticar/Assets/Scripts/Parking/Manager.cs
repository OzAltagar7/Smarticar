using System.Collections;
using UnityEngine;

/// <summary>
/// A class for managing the game.
/// </summary>
public class Manager : MonoBehaviour
{
    /// <summary>
    /// An array holding both agents of the match.
    /// </summary>
    public ParkingAgent[] cars;

    /// <summary>
    /// A reference to the UI manager.
    /// </summary>
    public ParkingUIManager UIManager;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        // Pause the game for the instructions phase
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// Start the game when clicking on the start button.
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
        float totalTime = 3f;

        while (totalTime > 0)
        {
            if (totalTime < 1) UIManager.countdownTimer.text = "GO";
            else UIManager.countdownTimer.text = totalTime.ToString("0");
            totalTime -= 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        // Hide all panels
        UIManager.countdownTimer.gameObject.SetActive(false);
        UIManager.darkPanel.SetActive(false);

        // Show timer
        UIManager.gameTimer.Show(true);

        // Start the game by resetting timeScale to 1f
        Time.timeScale = 1f;

        // the player entered the gameplay phase
        ParkingUIManager.duringGameplay = true;
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Display gameover screen
        StartCoroutine(UIManager.DisplayGameoverScreen(cars[0], cars[1]));
    }
}
