using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    public Image[] starImages; // An array of Image UI elements to display the stars

    private void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ScoreScene")
        {
            // Load the score, high score, and star count from PlayerPrefs
            int score = PlayerPrefs.GetInt("Score", 0);
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            int starCount = PlayerPrefs.GetInt("Stars", 0);

            // Update UI elements with the loaded data
            UpdateScoreText(score);
            UpdateHighScoreText(highScore);
            UpdateStarDisplay(starCount);
        }
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateHighScoreText(int highScore)
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void UpdateStarDisplay(int starCount)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < starCount)
            {
                starImages[i].enabled = true; // Enable the star image to show the earned stars
            }
            else
            {
                starImages[i].enabled = false; // Disable the star image to hide unearned stars
            }
        }
    }
}
