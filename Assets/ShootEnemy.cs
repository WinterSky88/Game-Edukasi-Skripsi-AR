using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootEnemy : MonoBehaviour
{
    public Button shootBtn;
    public Camera fpsCam;
    public float damage = 10f;
    public Text scoreText;

    private int score = 0;
    private int starCount = 0;

    // Make sure the score is not reset when the scene changes
    private static bool isScoreInitialized = false;

    // Use this for initialization
    void Start()
    {
        if (!isScoreInitialized)
        {
            // Initialize the score if it's not already done
            score = 0;
            isScoreInitialized = true;
        }

        shootBtn.onClick.AddListener(onShoot);
    }

    void onShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Enemy target = hit.transform.GetComponent<Enemy>();

            if (target != null)
            {
                target.TakeDamage(damage);
                score++;
                string scoreString = score.ToString();
                scoreText.text = "Score: " + scoreString;

                // Check if the score is a multiple of 3 to give a star reward
                if (score % 15 == 0)
                {
                    GiveStarReward();
                }
            }
        }
    }

    void GiveStarReward()
    {
        starCount++;
        Debug.Log("Star Reward! Total Stars: " + starCount);
        // You can implement your logic here to display the stars or perform any other actions.
        // For example, you can update a star UI element or play a particle effect.
    }

    // Save the score when the scene changes
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Stars", starCount);
        PlayerPrefs.Save();
    }
}
