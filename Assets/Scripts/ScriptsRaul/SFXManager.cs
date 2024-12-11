using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource sfxObject;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLenght = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }
}
