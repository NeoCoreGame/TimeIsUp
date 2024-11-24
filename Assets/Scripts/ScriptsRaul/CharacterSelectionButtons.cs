using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionButtons : MonoBehaviour
{
    public GameObject character1Selection;
    public GameObject character2Selection;
    public GameObject character3Selection;

    public GameObject character1Info;
    public GameObject character2Info;
    public GameObject character3Info;

    public GameObject characterSelectElements;
    public GameObject settingsElements;

    public Color color;

    private int referenceID = 0;

    public void Character1Selection()
    {
        character1Info.SetActive(true);
        character2Info.SetActive(false);
        character3Info.SetActive(false);

        referenceID = 0;
        color.a = character1Selection.GetComponent<Image>().color.a;
        color.a = 0.5f;
        character1Selection.GetComponent<Image>().color = color;
        color.a = 0f;
        character2Selection.GetComponent<Image>().color = color;
        character3Selection.GetComponent<Image>().color = color;
    }

    public void Character2Selection()
    {
        character1Info.SetActive(false);
        character2Info.SetActive(true);
        character3Info.SetActive(false);

        referenceID = 1;
        color.a = character2Selection.GetComponent<Image>().color.a;
        color.a = 0.5f;
        character2Selection.GetComponent<Image>().color = color;
        color.a = 0f;
        character1Selection.GetComponent<Image>().color = color;
        character3Selection.GetComponent<Image>().color = color;
    }

    public void Character3Selection()
    {
        character1Info.SetActive(false);
        character2Info.SetActive(false);
        character3Info.SetActive(true);

        referenceID = 2;
        color.a = character3Selection.GetComponent<Image>().color.a;
        color.a = 0.5f;
        character3Selection.GetComponent<Image>().color = color;
        color.a = 0f;
        character1Selection.GetComponent<Image>().color = color;
        character2Selection.GetComponent<Image>().color = color;
    }

    public void ToggleSettings()
    {
        if (!settingsElements.activeSelf)
        {
            characterSelectElements.SetActive(false);
            settingsElements.SetActive(true);
        }
        else
        {
            characterSelectElements.SetActive(true);
            settingsElements.SetActive(false);
        }
    }

    public void ConfirmButton()
    {
        //Enviar referenceID
        if (referenceID != 1)
        {
            StaticData.characterID = referenceID;
            SceneManager.LoadScene(2);
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
}
