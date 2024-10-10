using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public interface IShootable
{
    public void TakeDamage(int dmg);

    public int GetHealth();

    public float GetTimeReward();
}
public class Enemy : NetworkBehaviour, IShootable
{

    [SerializeField]
    public NetworkVariable<int> Hp = new NetworkVariable<int>();

    public TextMeshPro hpText;

    public int TimeReward;

    private void Start()
    {

        hpText.text = Hp.Value.ToString();

        Hp.OnValueChanged += OnDamageTaken;
    }

    private void OnDamageTaken(int previousValue, int newValue)
    {       
        hpText.text = Hp.Value.ToString();

        if(Hp.Value <= 0)
        {
            //Destroy(gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        OnTakeDamageServerRpc(dmg);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnTakeDamageServerRpc(int dmgTaken)
    {
        ServerGameManager.Instance.DamageEnemy(this, dmgTaken);
    }

    public int GetHealth()
    {
        return Hp.Value;
    }

    public float GetTimeReward()
    {
        return TimeReward;
    }
}
