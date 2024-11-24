using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InitialCountdown : NetworkBehaviour
{
    private TextMeshProUGUI counter;
    public RectTransform iCRect;

    [SerializeField]
    public NetworkVariable<int> contador = new NetworkVariable<int>(4);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }

    public void StartCounter()
    {
        counter = transform.GetChild(2).transform.GetComponent<TextMeshProUGUI>();
        iCRect = GetComponent<RectTransform>();
        contador.OnValueChanged += OnCounterChange;
    }

    private void Update()
    {
        if (contador.Value == 0 && iCRect.localScale != Vector3.zero)
        {
            iCRect.localScale = Vector3.zero;
        }
    }

    public void SubstractOne()
    {
        contador.Value--;
    }

    public void UpdateCounterText()
    {
        counter.text = contador.Value.ToString();
    }
    private void OnCounterChange(int previousValue, int newValue)
    {
        if (newValue > 0)
        {
            UpdateCounterText();
        }
    }
}
