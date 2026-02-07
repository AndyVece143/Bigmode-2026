using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public LevelLoader loader;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming");
        Resume();
    }

    public void LoadMenu()
    {
        Debug.Log("Menu");
        Resume();
        loader.LoadNextLevel("Title");
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting...");
        Resume();
        string currentSceneName = SceneManager.GetActiveScene().name;
        loader.LoadNextLevel(currentSceneName);
    }
}