using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkMusicToFader : MonoBehaviour
{
    public AudioSource audioSource;
    public RawImage fader;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    void Update() {
        audioSource.volume = Mathf.Lerp(musicVolume, 0f, fader.color.a);
    }
}
