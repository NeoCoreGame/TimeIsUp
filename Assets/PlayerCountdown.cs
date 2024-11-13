using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCountdown : NetworkBehaviour
{

    public NetworkVariable<float> avaliableTime = new NetworkVariable<float>();
    public TextMeshProUGUI timerText;

    private AddedTimeUI _addedTimeUI;


    private void Update()
    {
        if (IsOwner)
        {
            ShowTime(avaliableTime.Value);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        timerText = GameObject.FindGameObjectWithTag("PlayerCountdown").GetComponent<TextMeshProUGUI>();
        _addedTimeUI = FindObjectOfType<AddedTimeUI>();


        avaliableTime.OnValueChanged += OnCounterChange;

    }

    public string ShowTime(float timerCount)
    {
        //Calculo minutos, segundos y milisegundos de vuelta
        int minutes = (int)(timerCount / 60f);
        int seconds = (int)(timerCount - 60f * minutes);
        int cents = (int)((timerCount - minutes * 60f - seconds) * 100f);

        return timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }
    public void OnCounterChange(float previousValue, float newValue)
    {
        if (newValue < 0f && GetComponent<HealthController>().DeadScreen.localScale != Vector3.one)
        {
            GetComponent<HealthController>().DeadScreen.localScale = Vector3.one;


        }
    }

    public void TimeVariation(float timeVariation)
    {
        

        _addedTimeUI.LaunchText(timeVariation);
        TimeVariationServerRpc(timeVariation);


    }

    [ServerRpc(RequireOwnership = false)]
    private void TimeVariationServerRpc(float timeVariation)
    {
        avaliableTime.Value += timeVariation;

        

    }
}
