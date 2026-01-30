using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip mainLoop;
    public List<AudioClip> layersLoop;  
    void Start()
    {
        AudioManager.Instance.PlayLayeredMusic(mainLoop, layersLoop.ToArray());
    }
}
