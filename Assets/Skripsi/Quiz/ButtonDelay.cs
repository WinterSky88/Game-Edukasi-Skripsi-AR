using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDelay : MonoBehaviour
{
    public GameObject button; // Reference to the button GameObject
    public float delayTime = 10f; // Delay time in seconds before the button appears

    private void Start()
    {
        StartCoroutine(ActivateButtonAfterDelay());
    }

    private IEnumerator ActivateButtonAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        button.SetActive(true); // Activate the button after the specified delay
    }
}
