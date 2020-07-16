using System.Collections;
using UnityEngine;
using TMPro;

public class ParkingUIManager : MonoBehaviour
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
    /// The timer of the game.
    /// </summary>
    public ParkingTimer gameTimer;

    /// <summary>
    /// The user score (number of parkings).
    /// </summary>
    public TextMeshPro userScore;

    /// <summary>
    /// /// The AI score (number of parkings).
    /// </summary>
    public TextMeshPro AIScore;

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
    /// the game over text of the user score
    /// </summary>
    public TextMeshProUGUI userTotalParkings;

    /// <summary>
    /// the game over text of the AI score
    /// </summary>
    public TextMeshProUGUI AITotalParkings;

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
    /// Update the score of a given agent.
    /// </summary>
    /// <param name="parkingAgent">The agent to update it's score count.</param>
    public void UpdateScore(ParkingAgent parkingAgent)
    {
        if (parkingAgent.isUser)
        {
            userScore.text = parkingAgent.Score.ToString();
        }

        else
        {
            AIScore.text = parkingAgent.Score.ToString();
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
    public IEnumerator DisplayGameoverScreen(ParkingAgent user, ParkingAgent AI)
    {
        duringGameplay = false;

        // Wait 1 second before displaying the dark screen and the game over text
        yield return new WaitForSecondsRealtime(1);
        gameTimer.Show(false);
        darkPanel.SetActive(true);
        gameoverDefaultText.SetActive(true);

        // Display the correct image according to the user winning status
        if (user.Score > AI.Score) youWonImage.SetActive(true);
        else youLostImage.SetActive(true);

        // Wait 1 second before displaying the game stat
        yield return new WaitForSecondsRealtime(1);
        userTotalParkings.text = userScore.text;
        userTotalParkings.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1);
        AITotalParkings.text = AIScore.text;
        AITotalParkings.gameObject.SetActive(true);

        // Wait 1 second and display the quit button
        yield return new WaitForSecondsRealtime(1);
        quitButton.SetActive(true);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Pause only if the player is during the game (not in the instructions or gameover phase)
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
