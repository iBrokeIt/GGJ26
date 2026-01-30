using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Drag the AudioSource for Background Music here")]
    public AudioSource musicSource;
    
    [Tooltip("Drag the AudioSource for Sound Effects here")]
    public AudioSource sfxSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
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

    void Start()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
    
    public void PlayRandomizedSFX(AudioClip clip, bool randomize = true)
    {
        if (clip == null) return;

        float randomPitch = Random.Range(0.9f, 1.1f);
        sfxSource.pitch = randomPitch;
        sfxSource.PlayOneShot(clip, sfxVolume);
        sfxSource.pitch = 1f; 
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}