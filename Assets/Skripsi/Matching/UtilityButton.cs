// The following three lines import necessary libraries and packages
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This line defines a class called UtilityButton that inherits from the MonoBehaviour class in Unity
public class UtilityButton : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        // Check if there are any game objects with the tag "Phones"
        if (GameObject.FindGameObjectsWithTag("Phones").Length > 0)
        {
            // If there are, destroy the second phone object in the list (index 1)
            Destroy(GameObject.FindGameObjectsWithTag("Phones")[1]);
        }
    }

    // This method loads a specified scene by name when called
    public void SceneLoad(string Scenename)
    {
        SceneManager.LoadScene(Scenename);
    }

    // This method changes the size of phone objects based on the value of a slider
    public void PhoneSizeChange(float val)
    {
        // Get the value of the slider attached to this script's game object
        val = gameObject.GetComponent<Slider>().value;

        // Increase the value by 0.5
        val += 0.5f;

        // Create a new Vector3 object with val as the X, Y, and Z values
        Vector3 size = new Vector3(val, val, val);

        // Scale the game object with the tag "Phones" using the new vector
        GameObject.FindGameObjectWithTag("Phones").transform.localScale = size;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
