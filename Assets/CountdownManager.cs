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

    private void Start()
    {
        _networkManager = NetworkManager.Singleton;
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

                if(_contador.avaliableTime.Value <= 0f)
                {
                    //contadores.Remove(_contador);
                    //Destroy(_contador.gameObject);
                    _contador.gameObject.GetComponent<PlayerInput>().enabled = false;
                    _contador.gameObject.GetComponent<PlayerController>().enabled = false;
                    gameObject.transform.position = new Vector3(0, 3000f, 0);
                }

            }
        }
    }
}
