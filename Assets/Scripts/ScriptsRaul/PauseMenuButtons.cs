using System.Collections;
using System.Collections.Generic;
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
        pauseMenuElements = GameObject.Find("PauseMenuElements");

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

    public void ExitGame()
    {
        SceneManager.LoadScene(1);
    }
}
