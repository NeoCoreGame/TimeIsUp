using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
    public NetworkVariable<int> Hp = new NetworkVariable<int>(100);


    public bool hitted;
    public int hpThreshold;
    public int TimeReward;
    public int dmg;

    EnemySystem _enemySystem;

    public Image sliderFill;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        InitializeEnemy();

    }
    public void InitializeEnemy()
    {
        sliderFill.fillAmount = 1;
        if (IsServer) { Hp.Value = 100; }

        Hp.OnValueChanged += OnDamageTaken;
        // hpText.text = Hp.Value.ToString();


        _enemySystem = FindObjectOfType<EnemySystem>();

    }

    private void OnDamageTaken(int previousValue, int newValue)
    {
        hitted = true;
        //hpText.text = Hp.Value.ToString();

        sliderFill.fillAmount = newValue / 100f;

        if (Hp.Value <= 0)
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
