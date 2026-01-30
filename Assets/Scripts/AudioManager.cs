using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    
    private AudioSource baseMusicSource;
    private List<AudioSource> layerSources = new List<AudioSource>();

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Tooltip("How fast the layers fade in/out (seconds)")]
    public float layerFadeDuration = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            baseMusicSource = gameObject.AddComponent<AudioSource>();
            baseMusicSource.loop = true;
            baseMusicSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        baseMusicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
    
    public void PlayRandomizedSFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(clip, sfxVolume);
        sfxSource.pitch = 1f; 
    }

    public void PlayLayeredMusic(AudioClip baseClip, AudioClip[] layerClips)
    {
        StopMusic(); 

        baseMusicSource.clip = baseClip;
        baseMusicSource.volume = musicVolume; 

        while (layerSources.Count < layerClips.Length)
        {
            var newSource = gameObject.AddComponent<AudioSource>();
            newSource.loop = true;
            newSource.playOnAwake = false;
            layerSources.Add(newSource);
        }

        for (int i = 0; i < layerSources.Count; i++)
        {
                layerSources[i].clip = layerClips[i];
                layerSources[i].volume = 0f;
        }

        double startTime = AudioSettings.dspTime + 0.1;
        
        baseMusicSource.PlayScheduled(startTime);
        for (int i = 0; i < layerClips.Length; i++)
        {
            layerSources[i].PlayScheduled(startTime);
        }
    }

    public void SetActiveMusicLayer(int layerIndex)
    {
        for (int i = 0; i < layerSources.Count; i++)
        {
            if (layerSources[i].clip == null) continue;

            float targetVol = (i == layerIndex) ? musicVolume : 0f;
            StartCoroutine(FadeSource(layerSources[i], targetVol));
        }
    }

    private IEnumerator FadeSource(AudioSource source, float targetVolume)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < layerFadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / layerFadeDuration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void StopMusic()
    {
        baseMusicSource.Stop();
        foreach (var source in layerSources)
        {
            source.Stop();
        }
    }
}