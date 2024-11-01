using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : NetworkBehaviour
{

    [SerializeField]
    public NetworkVariable<int> HP = new NetworkVariable<int>();

    public GameObject healthStuff;

    public Slider hpSlider;
    public TextMeshProUGUI hpText;

    private CameraHit _camhit;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {

            healthStuff = GameObject.FindGameObjectWithTag("HealthStuff");
            hpSlider = healthStuff.transform.GetChild(0).GetComponent<Slider>();
            hpText = healthStuff.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            OnHealthChange(0, HP.Value);

            _camhit = Camera.main.GetComponent<CameraHit>();
            _camhit.InitializeScript(new Vector3(0f, 0.6f, 0.5f));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            HP.OnValueChanged += OnHealthChange;

            UpdateHealth(HP.Value);
        }
    }

    public void UpdateHealth(int hpValue)
    {
        hpSlider.value = ((float)hpValue) * .01f;
        hpText.text = hpValue.ToString();
    }

    private void OnHealthChange(int previousValue, int newValue)
    {
        UpdateHealth(newValue);

        if (previousValue > newValue)
        {
            _camhit.GetHit();
        }
    }
}
