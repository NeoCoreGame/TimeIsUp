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
    ServerMessageManager _messageManager;


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
        _messageManager = GameObject.FindGameObjectWithTag("ServerMessages").GetComponent<ServerMessageManager>();
    }

    private void OnClientConnected(ulong obj) //Ocurren en server y cliente
    {
        if (_networkManager.IsServer)
        {
            GameObject player = Instantiate(_playerPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(obj); //Ocurre en servidor

            player.transform.position = new Vector3(0f,3f,0f);

            string playerID = player.GetComponent<NetworkObject>().OwnerClientId.ToString();
            player.GetComponent<PlayerController>()._playerNumber.text = playerID;

            HealthController hC = player.GetComponent<HealthController>();
            hC.HP.Value = 100;

            _countdownManager.StartCounter(player.GetComponent<PlayerCountdown>());

            _messageManager.ServerMessage("+ Cliente " + playerID + " conectado");
        }
    }


    private void OnServerStarted()
    {

        _messageManager.ServerMessage("+ Servidor operativo");
    }

    public void DamageEnemy(Enemy enemy, int dmg)
    {
        if (_networkManager.IsServer)
        {
            enemy.Hp.Value -= dmg;
        }
    }


    public CountdownManager GetCoundownManager()
    {
        return _countdownManager;
    }
}
