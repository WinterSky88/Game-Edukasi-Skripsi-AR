using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Image[] starImages; // Array of star images (1 star, 2 stars, 3 stars, etc.)
    public Sprite[] starSprites; // Array of star sprites (0 stars, 1 star, 2 stars, etc.)

    private int score = 0;
    private int highScore = 0;

    // Static instance to ensure only one GameManager exists
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Make the GameManager a singleton (ensure only one instance exists)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Check if we are in the Score Scene and set the starImages references
        if (SceneManager.GetActiveScene().name == "ScoreScene")
        {
            // Find all the star images in the Score Scene and assign them to starImages array
            starImages = GameObject.FindObjectsOfType<Image>();

            // Load the high score from PlayerPrefs
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            UpdateStarImage();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        UpdateStarImage();

        // Update the high score if the current score is higher
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void UpdateStarImage()
    {
        int starsEarned = Mathf.Clamp(score / 10, 0, starSprites.Length - 1);

        // Show the appropriate star images based on the score earned
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].gameObject.SetActive(i <= starsEarned);
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }
}
