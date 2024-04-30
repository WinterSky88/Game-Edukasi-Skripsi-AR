using UnityEngine;
using UnityEngine.UI;

public class ScoreSceneScript : MonoBehaviour
{
    public Text highScoreText;

    void Start()
    {
        int highScore = GameManager.Instance.GetHighScore();
        highScoreText.text = "High Score: " + highScore.ToString();
    }
}
