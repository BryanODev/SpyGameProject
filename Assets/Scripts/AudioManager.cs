using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    AudioSource audioSource;
    public AudioClip escapeMusicClip;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShotClip(AudioClip clip) 
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayEscapeMusic() 
    {
        audioSource.clip = escapeMusicClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
