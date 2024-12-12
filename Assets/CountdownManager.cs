using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CountdownManager : MonoBehaviour
{
    private NetworkManager _networkManager;

    public List<PlayerCountdown> contadores;

    private float startingTime = 360f;
    //private float startingTime = 40f;
    private RespawnPlayerManager _respawnPlayerManager;

    private LobbyManager _lobbyManager;

    public GameObject youWon;

    public NetworkVariable<bool> finishedGame = new NetworkVariable<bool>();

    private void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _respawnPlayerManager = FindObjectOfType<RespawnPlayerManager>();

        _lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void StartCounter(PlayerCountdown _playerCountdown)
    {
        contadores.Add(_playerCountdown);
        _playerCountdown.avaliableTime.Value = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_networkManager.IsServer && !finishedGame.Value && _lobbyManager.startCountdown.iCRect.localScale == Vector3.zero)
        {
            foreach (PlayerCountdown _contador in contadores)
            {

                if (_contador.GetComponent<PlayerController>().enabled && _contador.avaliableTime.Value >= 0f)
                {
                    _contador.avaliableTime.Value -= Time.deltaTime; 
                }

                if(_contador.avaliableTime.Value <= 0f && _contador.GetComponent<PlayerController>().enabled)
                {
                    _contador.gameObject.GetComponent<PlayerInput>().enabled = false;
                    _contador.gameObject.GetComponent<PlayerController>().enabled = false;
                    _respawnPlayerManager.RespawnPlayer(_contador.GetComponent<HealthController>());

                    _contador.avaliableTime.Value = 0f;
                  
                }

            }

            if (!finishedGame.Value)
            {
                if (contadores.Count == 1 && CheckAlivePlayers() <= 0)
                {
                    //_lobbyManager.ReturnToLobby();
                    ResetCounters();

                    Invoke("ReturnToLobby", 2f);
                    finishedGame.Value = true;
                }
                if (contadores.Count > 1 && CheckAlivePlayers() <= 1)
                {
                    //_lobbyManager.ReturnToLobby();
                    ResetCounters();
                    Invoke("ReturnToLobby", 2f);
                    finishedGame.Value = true;
                } 
            }

        }
        if (finishedGame.Value)
        {
            youWon.SetActive(true);
        }
        else
        {
            youWon.SetActive(false);
        }
        if (_lobbyManager.returnPlayersToLobby.Value)
        {
            _lobbyManager.ReturnToLobby();
        }
    }

    public void ReturnToLobby()
    {

        _lobbyManager.returnPlayersToLobby.Value = true;
        finishedGame.Value = false;
    }

    public int CheckAlivePlayers()
    {
        int alivePlayers = 0;

        foreach(PlayerCountdown c in contadores)
        {
            if(c.avaliableTime.Value > 0f) { alivePlayers++; }
        }

        return alivePlayers;
    }

    public void ResetCounters()
    {
        foreach(PlayerCountdown playerCountdown in contadores) { playerCountdown.avaliableTime.Value = 360f; }
    }

    public void GoOutNet()
    {
        Destroy(NetworkManager.Singleton.gameObject);
      
    }
}
