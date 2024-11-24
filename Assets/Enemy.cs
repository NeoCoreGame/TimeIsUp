using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public interface IShootable
{
    public void TakeDamage(int dmg);

    public void Stun(float time);

    public int GetHealth();

    public float GetTimeReward();
}
public class Enemy : NetworkBehaviour, IShootable
{

    [SerializeField]
    public NetworkVariable<int> Hp = new NetworkVariable<int>();

    public TextMeshPro hpText;

    public bool hitted;
    public int hpThreshold;
    public int TimeReward;
    public int dmg;

     EnemySystem _enemySystem;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        InitializeEnemy();

    }
    public void InitializeEnemy()
    {
        

        hpText.text = Hp.Value.ToString();

        Hp.OnValueChanged += OnDamageTaken;

        _enemySystem = FindObjectOfType<EnemySystem>();

        if(IsServer) { GetComponent<enemyBehaviour>().enabled = true; }
    }

    private void OnDamageTaken(int previousValue, int newValue)
    {
        hitted = true;
        hpText.text = Hp.Value.ToString();

        if(Hp.Value <= 0)
        {
            //Destroy(gameObject);
        }
    }
    public void Stun(float time)
    {
        throw new NotImplementedException();
    }

    public bool GetHit()
    {
        return hitted;
    }
    public int GetDmg()
    {
        return dmg;
    }

    public int GetHealth()
    {
        return Hp.Value;
    }

    public float GetTimeReward()
    {
        return TimeReward;
    }

    public void TakeDamage(int dmg)
    {
        _enemySystem.EnemyTakeDamage(gameObject, dmg);
    }

    
}
