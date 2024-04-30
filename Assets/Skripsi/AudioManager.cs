using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;
    public AudioClip[] songs;
    public float fadeDuration = 1.0f;

    private List<AudioClip> shuffledSongs = new List<AudioClip>();
    private int currentSongIndex = 0;
    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ShuffleSongs();
        PlaySong(currentSongIndex);
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isFading)
        {
            NextSong();
        }
    }

    public void PlaySong(int songIndex)
    {
        if (songIndex >= 0 && songIndex < shuffledSongs.Count)
        {
            StartCoroutine(FadeOutAndPlay(songIndex));
        }
    }

    private void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % shuffledSongs.Count;
        PlaySong(currentSongIndex);
    }

    private void ShuffleSongs()
    {
        shuffledSongs = new List<AudioClip>(songs);
        for (int i = shuffledSongs.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            AudioClip temp = shuffledSongs[i];
            shuffledSongs[i] = shuffledSongs[randomIndex];
            shuffledSongs[randomIndex] = temp;
        }
    }

    private IEnumerator FadeOutAndPlay(int songIndex)
    {
        isFading = true;

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = shuffledSongs[songIndex];
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        isFading = false;
    }
}
