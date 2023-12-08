using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    public void PlaySfx(AudioClip clip){
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    public AudioSource AudioSource{
        get => audioSource;
        set => audioSource = value;
    }
}
