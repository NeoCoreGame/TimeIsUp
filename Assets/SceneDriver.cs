using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDriver : MonoBehaviour
{
    public void GoToCharacterSelect()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene(2);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
