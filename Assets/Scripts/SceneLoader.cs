using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A helper class for loading scenes asynchronously and displaying a loading bar.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// The screen for the loading process.
    /// </summary>
    public GameObject loadingScreen;

    /// <summary>
    /// The loading bar slider.
    /// </summary>
    public Slider progressSlider;

    /// <summary>
    /// Load a given scene.
    /// </summary>
    /// <param name="sceneName">The scene to load.</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    /// <summary>
    /// Load a given scene asynchronously and display a loading bar;
    /// </summary>
    /// <returns>
    /// Waits for the loading to finish.
    /// </returns>
    /// <param name="sceneName">The scene to load.</param>
    IEnumerator LoadAsync (string sceneName)
    {
        // Load the scene asynchronously
        AsyncOperation sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneName);

        // While the scene is loading, if the scene is either the racetrack or the parking mode,
        // display the loading progress bar.
        if (sceneName != "ChooseMode" && sceneName != "MainMenu")
        {
            loadingScreen.SetActive(true);

            while (!sceneLoadingOperation.isDone)
            {
                float loadingProgress = Mathf.Clamp01(sceneLoadingOperation.progress / 0.9f);
                progressSlider.value = loadingProgress;
                yield return null;
            }
        }
    }
}
