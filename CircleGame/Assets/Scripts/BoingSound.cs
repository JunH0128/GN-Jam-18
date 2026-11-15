using UnityEngine;


public class BoingSound : MonoBehaviour
{
    public AudioClip jump, land;
    public float volume = 0.6f;

    AudioSource source;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Jump() { if (jump != null) source.PlayOneShot(jump, volume); }
    public void Land() { if (land != null) source.PlayOneShot(land, volume); }
}
