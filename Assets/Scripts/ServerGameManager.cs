using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Qos.V2.Models;
using UnityEngine;

public class ServerGameManager : MonoBehaviour
{
    public static ServerGameManager Instance { get; private set; } //Instancia del singleton

    NetworkManager _networkManager;
    GameObject _playerPrefab;

    CountdownManager _countdownManager;
    RespawnPlayerManager _respawnPlayerManager;
    ServerMessageManager _messageManager;
    EnemySystem _enemySystem;

    private LobbyManager _lobbyManager;


    void Awake() //Inicializa la instancia del singleton
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;

        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;

        _countdownManager = GetComponent<CountdownManager>();
        _respawnPlayerManager = GetComponent<RespawnPlayerManager>();
        _messageManager = GameObject.FindGameObjectWithTag("ServerMessages").GetComponent<ServerMessageManager>();
        _enemySystem = FindObjectOfType<EnemySystem>();


        _lobbyManager = FindObjectOfType<LobbyManager>();

    }

    private void OnClientConnected(ulong obj) //Ocurren en server y cliente
    {
        if (_networkManager.IsServer)
        {
            GameObject player = Instantiate(_playerPrefab);

            string playerID = player.GetComponent<NetworkObject>().NetworkObjectId.ToString();
            player.GetComponent<PlayerInfo>().SetPlayerName("Player");

            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj); //Ocurre en servidor

            _lobbyManager.AddConnectedPlayers(player);


            player.GetComponent<PlayerController>()._playerNumber.text = playerID;

            _respawnPlayerManager.StartHealth(player.GetComponent<HealthController>());

            _countdownManager.StartCounter(player.GetComponent<PlayerCountdown>());

            _messageManager.ServerMessage("+ Cliente " + playerID + " conectado");

            if (_countdownManager.contadores.Count == 1)
            {
                _enemySystem.InitializePools();

            }
        }
    }


    private void OnServerStarted()
    {

        _messageManager.ServerMessage("+ Servidor operativo");
    }

    public NetworkManager GetNetworkManager()
    {
        return _networkManager;
    }
    public NetworkManager GetEnemySystem()
    {
        return _networkManager;
    }
    public CountdownManager GetCoundownManager()
    {
        return _countdownManager;
    }



}
