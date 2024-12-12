using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    GameObject pauseMenuCanvas;
    GameObject pauseMenuElements;
    GameObject settingsMenuElements;

    private void Awake()
    {

        pauseMenuCanvas = GameObject.Find("PauseMenuCanvas");
        settingsMenuElements = GameObject.Find("SettingsMenuElements");
        pauseMenuElements = GameObject.Find("PauseMenu");

    }

    private void Start()
    {

        settingsMenuElements.SetActive(false);
        //pauseMenuElements.SetActive(false);
        pauseMenuCanvas.SetActive(false);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuCanvas.activeInHierarchy)
        {
            pauseMenuCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }   
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuCanvas.activeInHierarchy)
        {
            settingsMenuElements.SetActive(false);
            pauseMenuElements.SetActive(true);
            pauseMenuCanvas.SetActive(false);
        }
    }

    public void Resume()
    {

        pauseMenuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Settings()
    {

        pauseMenuElements.SetActive(false);
        settingsMenuElements.SetActive(true);

    }

    public void SettingsBack()
    {
        pauseMenuElements.SetActive(true);
        settingsMenuElements.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (!settingsMenuElements.activeSelf)
        {
            settingsMenuElements.SetActive(true);
        }
        else
        {
            settingsMenuElements.SetActive(false);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(1);
    }
}
