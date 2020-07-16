using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// A Manager for handling all of the UI stuff in the Racetrack mode.
/// </summary>
public class RacetrackUIManager : MonoBehaviour
{
    /// <summary>
    /// The dark panel much of the UI is displayed on (e.i instructions, gameover).
    /// </summary>
    public GameObject darkPanel;

    // Adds a header in the inspector
    [Header("Starting")]

    /// <summary>
    /// The instructions game object consisting of the tutorial text and the images.
    /// </summary>
    public GameObject instructions;

    /// <summary>
    /// The starting countdown timer text. 
    /// </summary>
    public TextMeshProUGUI countdownTimer;

    [Header("Gameplay")]

    /// <summary>
    /// The timer of the race.
    /// </summary>
    public RaceTimer raceTimer;

    /// <summary>
    /// The user lap count text
    /// </summary>
    public TextMeshProUGUI userLapCount;

    /// <summary>
    /// The AI lap count text
    /// </summary>
    public TextMeshProUGUI AILapCount;

    /// <summary>
    /// A pause menu appearing when pressing ESC.
    /// </summary>
    public GameObject pauseMenu;

    /// <summary>
    /// The mute button text.
    /// </summary>
    public TextMeshProUGUI muteButtonText;

    /// <summary>
    /// Whether or not the user is currently playing (not in the instructions phase or the game over phase).
    /// </summary>
    [HideInInspector]
    public static bool duringGameplay;

    /// <summary>
    /// Set to true if the game is currently paused.
    /// </summary>
    [HideInInspector]
    public static bool gameIsPaused = false;

    [Header("Game Over")]

    /// <summary>
    /// The game over screen default text.
    /// </summary>
    public GameObject gameoverDefaultText;

    /// <summary>
    /// The "YOU WON" image displayed when the user wins.
    /// </summary>
    public GameObject youWonImage;

    /// <summary>
    /// The "YOU LOST" image displayed when the user wins.
    /// </summary>
    public GameObject youLostImage;

    /// <summary>
    /// The game over text of the number of laps the user made.
    /// </summary>
    public TextMeshProUGUI userLapsMade;

    /// <summary>
    /// The game over text of the number of laps the AI made.
    /// </summary>
    public TextMeshProUGUI AILapsMade;


    /// <summary>
    /// The game over text of the total time of the race.
    /// </summary>
    public TextMeshProUGUI totalTime;

    /// <summary>
    /// The game over quit button.
    /// </summary>
    public GameObject quitButton;

    /// <summary>
    /// A reference to the audio manager.
    /// </summary>
    [Space(10)]
    public AudioManager audioManager;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    /// <summary>
    /// Update the lap count of a given agent.
    /// </summary>
    /// <param name="carAgent">The agent to update it's lap count.</param>
    public void UpdateLapCount(CarAgent carAgent)
    {
        if (carAgent.isUser)
        {
            userLapCount.text = string.Format("LAP {0}/2", carAgent.LapsMade);
        }

        else
        {
            AILapCount.text = string.Format("LAP {0}/2", carAgent.LapsMade);
        }
    }

    /// <summary>
    /// Display the game over screen.
    /// </summary>
    /// <returns>
    /// Waits before displaying stats.
    /// </returns>
    /// <param name="user">The user agent.</param>
    /// <param name="AI">The AI agent.</param>
    public IEnumerator DisplayGameoverScreen(CarAgent user, CarAgent AI)
    {
        duringGameplay = false;

        // Wait 2 seconds before displaying the dark screen and the default game over text
        yield return new WaitForSeconds(2);
        darkPanel.SetActive(true);
        gameoverDefaultText.SetActive(true);

        // Display the correct image according to the user winning status
        if (user.LapsMade > AI.LapsMade) youWonImage.SetActive(true);
        else youLostImage.SetActive(true);

        // Wait 1 second before displaying the game stat
        yield return new WaitForSeconds(1);
        totalTime.text = raceTimer.GetTotalTime();
        totalTime.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        userLapsMade.text = string.Format("{0}/2", user.LapsMade);
        userLapsMade.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        AILapsMade.text = string.Format("{0}/2", AI.LapsMade);
        AILapsMade.gameObject.SetActive(true);

        // Wait 1 second and display the quit button
        yield return new WaitForSeconds(1);
        quitButton.SetActive(true);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && duringGameplay)
        {
            if (gameIsPaused) Resume();

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        darkPanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        darkPanel.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    /// <summary>
    /// Quit the application when pressing on the quit button.
    /// </summary>
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void MuteAudio()
    {
        if (AudioManager.audioIsMuted)
        {
            audioManager.SetVolume("MainTheme", 0.5f);
            AudioManager.audioIsMuted = false;
            muteButtonText.text = "MUTE";
        }

        else
        {
            audioManager.SetVolume("MainTheme", 0);
            AudioManager.audioIsMuted = true;
            muteButtonText.text = "UNMUTE";
        }
    }
}
