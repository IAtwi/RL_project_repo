using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to AudioSource
    public AudioClip[] playlist;     // Array of soundtracks
    public bool shuffle = true; // Shuffle toggle state
    public float fadeDuration = 1.5f;

    public Button toggleButton; // Mute button
    public Button shuffleButton; // Shuffle button

    public Sprite muteSprite;
    public Sprite unmuteSprite;
    public Sprite shuffleOnSprite;  // Icon for Shuffle ON
    public Sprite shuffleOffSprite; // Icon for Shuffle OFF

    private int currentTrackIndex = 0;
    private Coroutine fadeCoroutine;
    private bool isPlaying = true;

    void Start()
    {
        LoadPlaylist();
        if (playlist.Length == 0)
        {
            Debug.LogError("Playlist is empty! Add soundtracks to the AudioSystem.");
            return;
        }

        audioSource.loop = false;
        PlayNextTrack();

        // Connect buttons to their functions
        if (toggleButton != null) toggleButton.onClick.AddListener(ToggleMusic);
        if (shuffleButton != null) shuffleButton.onClick.AddListener(ToggleShuffle);

        // Update UI icons
        UpdateButtonSprite();
        UpdateShuffleButtonSprite();
    }

    void PlayNextTrack()
    {
        if (playlist.Length == 0) return;

        if (shuffle)
        {
            currentTrackIndex = Random.Range(0, playlist.Length);
        }
        else
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
        }

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeToNextTrack(playlist[currentTrackIndex]));
    }

    IEnumerator FadeToNextTrack(AudioClip newClip)
    {
        yield return StartCoroutine(FadeVolume(1f, 0f, fadeDuration / 2));

        audioSource.clip = newClip;
        audioSource.Play();

        yield return StartCoroutine(FadeVolume(0f, 1f, fadeDuration / 2));

        StartCoroutine(WaitForTrackToEnd(newClip.length - fadeDuration));
    }

    IEnumerator FadeVolume(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        audioSource.volume = to;
    }

    IEnumerator WaitForTrackToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        PlayNextTrack();
    }

    void ToggleMusic()
    {
        if (audioSource != null)
        {
            isPlaying = !isPlaying;
            audioSource.mute = !isPlaying;
            UpdateButtonSprite();
        }
    }

    void UpdateButtonSprite()
    {
        if (toggleButton != null)
        {
            Image buttonImage = toggleButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = isPlaying ? unmuteSprite : muteSprite;
            }
        }
    }

    void ToggleShuffle()
    {
        shuffle = !shuffle;
        UpdateShuffleButtonSprite();
        Debug.Log("Shuffle mode: " + (shuffle ? "ON" : "OFF"));
    }

    void UpdateShuffleButtonSprite()
    {
        if (shuffleButton != null)
        {
            Image buttonImage = shuffleButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = shuffle ? shuffleOnSprite : shuffleOffSprite;
            }
        }
    }

    void LoadPlaylist()
    {
        playlist = Resources.LoadAll<AudioClip>("MusicFolder");
    }
}
