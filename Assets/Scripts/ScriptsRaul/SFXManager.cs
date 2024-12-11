using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    

    public SettingsValues settingsValues = new SettingsValues();

    public class SettingsValues
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float sensitivity;
    }

    [SerializeField] private AudioSource sfxObject;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnTransform)
    {
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        GetSettingsValues(); 

        audioSource.clip = audioClip;

        audioSource.volume = settingsValues.sfxVolume;

        audioSource.Play();

        float clipLenght = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }

    public void GetSettingsValues()
    {
        try
        {
            string filePath = Application.persistentDataPath + "/SettingsData.json";
            string settingsData = System.IO.File.ReadAllText(filePath);
            settingsValues = JsonUtility.FromJson<SettingsValues>(settingsData);
        }
        catch
        {
            Debug.Log("Data not found");
        }

    }
}
