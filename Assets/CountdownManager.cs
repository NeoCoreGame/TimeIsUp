using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CountdownManager : MonoBehaviour
{
    private NetworkManager _networkManager;

    public List<PlayerCountdown> contadores;

    //private float startingTime = 360f;
    private float startingTime = 20f;
    private RespawnPlayerManager _respawnPlayerManager;

    private void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _respawnPlayerManager = FindObjectOfType<RespawnPlayerManager>();
    }

    public void StartCounter(PlayerCountdown _playerCountdown)
    {
        contadores.Add(_playerCountdown);
        _playerCountdown.avaliableTime.Value = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_networkManager.IsServer)
        {
            foreach (PlayerCountdown _contador in contadores)
            {

                _contador.avaliableTime.Value -= Time.deltaTime;

                if(_contador.avaliableTime.Value <= 0f && _contador.GetComponent<PlayerController>().enabled)
                {
                    _contador.gameObject.GetComponent<PlayerInput>().enabled = false;
                    _contador.gameObject.GetComponent<PlayerController>().enabled = false;
                    _respawnPlayerManager.RespawnPlayer(_contador.GetComponent<HealthController>());
                  
                }

            }
        }
    }
}
