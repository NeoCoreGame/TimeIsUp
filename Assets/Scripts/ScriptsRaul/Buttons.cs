using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    GameObject mainMenuElements;
    GameObject settingsMenuElements;
    GameObject creditsElements;
    GameObject controlsElements;
    GameObject shopElements;
    GameObject personalizationElements;
    GameObject battlePassElements;
    GameObject shopPreviewElements;


    private void Awake()
    {

        settingsMenuElements = GameObject.Find("SettingsMenuElements");
        mainMenuElements = GameObject.Find("MainMenuElements");
        creditsElements = GameObject.Find("CreditsElements");
        controlsElements = GameObject.Find("ControlsElements");
        shopElements = GameObject.Find("ShopElements");
        personalizationElements = GameObject.Find("PersonalizationElements");
        battlePassElements = GameObject.Find("BattlePassElements");
        shopPreviewElements = GameObject.Find("Preview");

    }
    private void Start()
    {
        shopPreviewElements.SetActive(false);
        settingsMenuElements.SetActive(false);
        creditsElements.SetActive(false);
        controlsElements.SetActive(false);
        shopElements.SetActive(false);
        personalizationElements.SetActive(false);
        battlePassElements.SetActive(false);
    }
    
    //MainMenu
    public void PlayButton()
    {

        SceneManager.LoadScene(1);

    }

    public void SettingsButton()
    {

        //Settings Menu Appears
        settingsMenuElements.SetActive(true); 

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);

    }

    public void ControlsButton()
    {

        //Settings Menu Appears
        controlsElements.SetActive(true);

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);

    }

    public void CreditsButton()
    {

        //Settings Menu Appears
        creditsElements.SetActive(true);

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);

    }

    public void ShopButton()
    {

        //Settings Menu Appears
        shopElements.SetActive(true);

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);
        battlePassElements.SetActive(false);
        personalizationElements.SetActive(false);

    }

    public void PersonalizationButton()
    {

        //Settings Menu Appears
        personalizationElements.SetActive(true);

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);
        battlePassElements.SetActive(false);
        shopElements.SetActive(false);

    }

    public void BattlePassButton()
    {

        //Settings Menu Appears
        battlePassElements.SetActive(true);

        //Main Menu Dissapears
        mainMenuElements.SetActive(false);
        shopElements.SetActive(false);
        personalizationElements.SetActive(false);

    }

    public void ExitGameButton()
    {

        Application.Quit();
        Debug.Log("Game quited");

    }

    //Settings
    public void SettingsBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        settingsMenuElements.SetActive(false);

    }

    //Credits
    public void CreditsBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Credits Menu Dissapears
        creditsElements.SetActive(false);

    }

    //Controls
    public void ControlsBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        controlsElements.SetActive(false);

    }

    //Shop
    public void ShopBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        shopElements.SetActive(false);

    }

    public void TogglePreview()
    {

        if (shopPreviewElements.activeSelf)
        {
            shopPreviewElements.SetActive(false);
        }
        else
        {
            shopPreviewElements.SetActive(true);
        }

    }

    //Personalization
    public void PersonalizationBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        personalizationElements.SetActive(false);

    }

    //BattlePass
    public void BattlePassBackButton()
    {

        //Main Menu Appears
        mainMenuElements.SetActive(true);

        //Settings Menu Dissapears
        battlePassElements.SetActive(false);

    }

}
