using UnityEngine;
using UnityEngine.UI;

public class DistanceChecker : MonoBehaviour
{
    public Transform player;
    public GameObject panel;
    public float distanceThreshold = 10f;

    private Text tooFarText;
    private Text inRangeText;

    private void Start()
    {
        // Find the child text objects in the panel
        tooFarText = panel.transform.Find("TooFarText").GetComponent<Text>();
        inRangeText = panel.transform.Find("InRangeText").GetComponent<Text>();

        // Disable the panel initially
        panel.SetActive(false);
    }

    private void Update()
    {
        // Calculate the distance between the game object and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Check if the distance is within the threshold
        if (distance <= distanceThreshold)
        {
            // Show the "InRangeText" and hide the "TooFarText"
            tooFarText.gameObject.SetActive(false);
            inRangeText.gameObject.SetActive(true);
        }
        else
        {
            // Show the "TooFarText" and hide the "InRangeText"
            tooFarText.gameObject.SetActive(true);
            inRangeText.gameObject.SetActive(false);
        }

        // Show/hide the panel based on the distance
        panel.SetActive(distance > distanceThreshold);
    }
}
