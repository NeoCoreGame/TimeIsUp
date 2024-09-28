using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ServerGameManager : MonoBehaviour
{
    NetworkManager _networkManager;
    GameObject _playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong obj) //Ocurren en server y cliente
    {
        if (_networkManager.IsServer)
        {
            GameObject player = Instantiate(_playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj); //Ocurre en servidor
            player.GetComponent<PlayerController>()._playerNumber.text = player.GetComponent<NetworkObject>().NetworkObjectId.ToString();
            print("Cliente Conectado");
        }
    }

    private void OnServerStarted()
    {
        print("El servidor está encendido");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
