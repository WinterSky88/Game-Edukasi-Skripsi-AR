using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootEnemy1 : MonoBehaviour
{
    public Button shootBtn;
    public Camera fpsCam;
    public float damage = 10f;
    public Text scoreText;

    private int score = 0;

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
            }
        }
    }

    // Save the score when the scene changes
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }
}
