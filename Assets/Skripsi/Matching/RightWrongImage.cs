// The following three lines import necessary libraries and packages
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This line defines a class called RightWrongImage that inherits from the MonoBehaviour class in Unity
public class RightWrongImage : MonoBehaviour
{
    // These two public variables will hold Sprite objects for matching and non-matching images
    public Sprite Match, NoMatch;

    // These two public variables will hold an AudioSource object and an array of AudioClip objects
    public AudioSource ad;
    public AudioClip[] audios;

    // Start is called before the first frame update
    void Start()
    {
        // Check if this game object's Image component's sprite is equal to the Match sprite
        if (gameObject.GetComponent<Image>().sprite == Match)
        {
            // If it is, set the AudioSource's clip to the first AudioClip in the array
            ad.clip = audios[0];
        }
        else
        {
            // Otherwise, set the AudioSource's clip to the second AudioClip in the array
            ad.clip = audios[1];
        }

        // Play the audio clip
        ad.Play();

        // Destroy this game object after 2.5 seconds
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // This method is empty and does nothing
    }
}
