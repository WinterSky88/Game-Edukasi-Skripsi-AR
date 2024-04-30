using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject pauseMenu;

    private void Start()
    {
        // Find the PauseMenu object in the scene (make sure it's active and placed inside the Canvas)
        pauseMenu = GameObject.Find("PauseMenu");
        // Hide the pause menu initially
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        // Check for input to pause/unpause the game (you can modify this as needed)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Pause/unpause the game by adjusting the time scale
        if (isPaused)
        {
            Time.timeScale = 0f;
            // Show the PauseMenu when the game is paused
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            // Hide the PauseMenu when the game is unpaused
            pauseMenu.SetActive(false);
        }
    }
}
