using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectMap : MonoBehaviour
{

    private LobbyManager _manager;
    public int mapIdx;

    private void Start()
    {
        _manager = FindObjectOfType<LobbyManager>();
    }


    public void SelecMapp()
    {

        _manager.SelectMap(mapIdx);
    }
}
