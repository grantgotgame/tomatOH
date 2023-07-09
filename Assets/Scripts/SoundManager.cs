using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip boing;
    public AudioClip cleaver;
    public AudioClip scream;
    public AudioClip squish;
    public AudioClip swoosh;
    public AudioClip trumpet;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Play a given sound
    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
