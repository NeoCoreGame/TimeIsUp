using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionButtons : MonoBehaviour
{
    public Character character1;
    public Character character2;
    public Character character3;

    public Character[] charactersArray;

    private int referenceID = 0;
    private int lastReferenceID = 0;


    //Character Selection Menu
    public void RightArrow()
    {
        lastReferenceID = referenceID;

        if (referenceID > charactersArray.Length-2)
        {
            referenceID = 0;
        }
        else
        {
            referenceID++;
        }
        
        
        CharacterShowed(referenceID, lastReferenceID);
    }

    public void LeftArrow()
    {
        lastReferenceID = referenceID;

        if (referenceID < 1)
        {
            referenceID = charactersArray.Length-1;
        }
        else
        {
            referenceID--;
        }
        

        CharacterShowed(referenceID, lastReferenceID);
    }

    public void CharacterShowed(int ID, int lastID)
    {
        charactersArray[lastID].gameObject.SetActive(false);
        charactersArray[ID].gameObject.SetActive(true);
    }

    public void ConfirmButton()
    {
        //Enviar referenceID
        StaticData.characterID = referenceID;
        SceneManager.LoadScene(3);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(1);
    }

    
}
