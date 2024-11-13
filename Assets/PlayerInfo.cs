using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string playerName;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
}
