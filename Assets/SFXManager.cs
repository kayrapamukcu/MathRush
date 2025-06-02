using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private AudioSource src;

    void Awake()
    {
        if (Instance == null) { Instance = this; src = GetComponent<AudioSource>(); }
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        src.PlayOneShot(clip, volume);
    }
}
