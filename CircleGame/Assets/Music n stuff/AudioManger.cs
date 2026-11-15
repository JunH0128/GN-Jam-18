using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] coinSounds;

    private AudioSource source;
    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;

        source = GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();
    }

    public void PlayNextCoinSound()
    {
        if (coinSounds == null || coinSounds.Length == 0) return;

        source.PlayOneShot(coinSounds[currentIndex]);
        currentIndex = (currentIndex + 1) % coinSounds.Length;
    }
}