using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    GameObject mainMenuElements;
    GameObject settingsMenuElements;


    private void Awake()
    {

        settingsMenuElements = GameObject.Find("SettingsMenuElements");
        mainMenuElements = GameObject.Find("MainMenuElements");

    }
    private void Start()
    {

        settingsMenuElements.SetActive(false);

    }

    public void PlayButton()
    {

        SceneManager.LoadScene(2);

    }

    public void SettingsButton()
    {

        //Settings Menu Appears
        settingsMenuElements.SetActive(true); 

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);

    }

    public void SettingsBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        settingsMenuElements.SetActive(false);

    }

    public void ExitGameButton()
    {

        Application.Quit();
        Debug.Log("Game quited");

    }

}
