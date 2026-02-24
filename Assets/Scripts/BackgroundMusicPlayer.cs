using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip mainLoop;
    public List<AudioClip> layersLoop;  
    void Start()
    {
        // Skip if this is a duplicate MANAGERS object about to be destroyed
        if (AudioManager.Instance != null && AudioManager.Instance.gameObject != gameObject)
            return;

        AudioManager.Instance.PlayLayeredMusic(mainLoop, layersLoop.ToArray());
    }
}
