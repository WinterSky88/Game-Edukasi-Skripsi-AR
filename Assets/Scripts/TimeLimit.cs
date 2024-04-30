using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    public float timeLimit = 60f;
    public Text timerText;
    public Button startButton;

    private float currentTime;
    private bool isTimerActive = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerText();
        startButton.onClick.AddListener(StartTimer);
    }

    void Update()
    {
        if (isTimerActive && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else if (currentTime <= 0f)
        {
            isTimerActive = false;

            // Update and save the high score
            int currentScore = PlayerPrefs.GetInt("Score", 0);
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (currentScore > highScore)
            {
                PlayerPrefs.SetInt("HighScore", currentScore);
                PlayerPrefs.Save();
            }

            // Load the Score Scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScene");
        }
    }

    void UpdateTimerText()
    {
        timerText.text = ""+ currentTime.ToString("F0");
    }

    public void StartTimer()
    {
        isTimerActive = true;
    }
}
