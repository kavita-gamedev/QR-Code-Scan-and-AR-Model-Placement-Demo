using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVoiceover : MonoBehaviour
{
    public AudioSource bGMusic;
    public void PlayVoiceoverAudio()
    {
        gameObject.GetComponent<AudioSource>().Play();
        bGMusic.Play();
    }

    public void StopVoiceOverAudio()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        bGMusic.Stop();
    }
}
