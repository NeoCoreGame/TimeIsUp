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
    public GameObject sensitivitySlider;

    public GameObject confirmationPanel;
    public GameObject settingsPanel;

    public GameObject player;


    public class SettingsValues
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float sensitivity;
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
        SetSensitivity(settingsValues.sensitivity);
    }

    public void SaveToJson()
    {

        string settingsData = JsonUtility.ToJson(settingsValues);
        string filePath = Application.persistentDataPath + "/SettingsData.json";
        Debug.Log(filePath);
        Debug.Log(settingsData);
        System.IO.File.WriteAllText(filePath, settingsData);
        Debug.Log("Data saved");
        confirmationPanel.SetActive(false);
        settingsPanel.SetActive(true);
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
            sensitivitySlider.GetComponent<Slider>().value = settingsValues.sensitivity;
            Debug.Log("Data loaded");
        }
        catch
        {
            Debug.Log("Data not found");
        }
        
    }

    public void SetMasterVolume (float masterVolume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20f);
        settingsValues.masterVolume = masterVolume;
    }

    public void SetMusicVolume(float musicVolume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20f);
        settingsValues.musicVolume = musicVolume;
    }

    public void SetSFXVolume(float sfxVolume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20f);
        settingsValues.sfxVolume = sfxVolume;
    }

    public void SetSensitivity(float sensitivity)
    {
        settingsValues.sensitivity = sensitivity;
        player.GetComponent<MouseLook>().UpdateMouseSensitivity(sensitivity);
    }

    public void ConfirmationToggle()
    {
        if (confirmationPanel.activeSelf == true)
        {
            settingsPanel.SetActive(true);
            confirmationPanel.SetActive(false);
        }
        else
        {
            confirmationPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }
}
