using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public SettingsValues settingsValues = new SettingsValues();
    public GameObject masterVolumeSlider;
    public GameObject musicVolumeSlider;
    public GameObject sfxVolumeSlider;


    public class SettingsValues
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
    }

    public void Awake()
    {
        LoadFromJson();
    }

    public void Start()
    {
        SetMasterVolume(settingsValues.masterVolume);
        SetMusicVolume(settingsValues.musicVolume);
        SetSFXVolume(settingsValues.sfxVolume);
    }

    public void SaveToJson()
    {

        string settingsData = JsonUtility.ToJson(settingsValues);
        string filePath = Application.persistentDataPath + "/SettingsData.json";
        Debug.Log(filePath);
        Debug.Log(settingsData);
        System.IO.File.WriteAllText(filePath, settingsData);
        Debug.Log("Data saved");
    }

    public void LoadFromJson()
    {
        try
        {
            string filePath = Application.persistentDataPath + "/SettingsData.json";
            string settingsData = System.IO.File.ReadAllText(filePath);
            settingsValues = JsonUtility.FromJson<SettingsValues>(settingsData);
            masterVolumeSlider.GetComponent<Slider>().value = settingsValues.masterVolume;
            musicVolumeSlider.GetComponent<Slider>().value = settingsValues.musicVolume;
            sfxVolumeSlider.GetComponent<Slider>().value = settingsValues.sfxVolume;
            Debug.Log("Data loaded");
        }
        catch
        {
            Debug.Log("Data not found");
        }
        
    }

    public void SetMasterVolume (float masterVolume)
    {
        audioMixer.SetFloat("MasterVolume", masterVolume);
        settingsValues.masterVolume = masterVolume;
    }

    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("MusicVolume", musicVolume);
        settingsValues.musicVolume = musicVolume;
    }

    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFXVolume", sfxVolume);
        settingsValues.sfxVolume = sfxVolume;
    }
}
