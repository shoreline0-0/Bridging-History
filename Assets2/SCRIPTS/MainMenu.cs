using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen; // To display a loading screen while scene is loading
    public UnityEngine.UI.Slider slider; // Optional: If you want a progress bar

    // Call this function to play the game
    public void PlayGame()
    {
        // Optionally show a loading screen before scene load
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        StartCoroutine(LoadSceneAsync(1)); // Replace 1 with your game scene's build index
    }

    // Coroutine to load scene asynchronously
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Don't let the scene activate until it's fully loaded
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Check if the scene is loaded
            if (operation.progress >= 0.9f)
            {
                // Optionally, activate the scene after it's fully loaded
                operation.allowSceneActivation = true;
            }

            // Optionally update progress bar or loading feedback
            if (slider != null)
            {
                slider.value = operation.progress;
            }

            yield return null;
        }
    }

    // Call this function to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
