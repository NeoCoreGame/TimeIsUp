using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectMap : MonoBehaviour, IPointerEnterHandler
{

    private LobbyManager _manager;
    public int mapIdx;

    private void Start()
    {
        _manager = FindObjectOfType<LobbyManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _manager.SelectMap(mapIdx);
    }
}
