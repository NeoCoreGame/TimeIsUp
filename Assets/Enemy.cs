using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;

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
    public NetworkVariable<int> Hp = new NetworkVariable<int>(300);


    public bool hitted;
    public int hpThreshold;
    public int TimeReward;
    public int dmg;

    EnemySystem _enemySystem;

    public Image sliderFill;

    public enum EnemyType
    {
        Minion, Escurridizo, Explosivo, Tanque, Volador
    }

    public EnemyType enemyType;
    private float ogHp;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        InitializeEnemy();

    }

    private void OnEnable()
    {
        if(NetworkManager.Singleton && NetworkManager.Singleton.IsServer) { Hp.Value = (int)ogHp; }
    }
    public void InitializeEnemy()
    {
        gameObject.GetComponent<IEnemyBehaviour>().EnableBehaviour();


        sliderFill.fillAmount = 1;
        if (NetworkManager.Singleton.IsServer)
        {
            switch (enemyType)
            {
                case EnemyType.Minion: Hp.Value = 100; break;

                case EnemyType.Escurridizo: Hp.Value = 50; break;

                case EnemyType.Explosivo: Hp.Value = 40; break;

                case EnemyType.Tanque: Hp.Value = 300; break;

                case EnemyType.Volador: Hp.Value = 200; break;

            }


        }

        Hp.OnValueChanged += OnDamageTaken;
        // hpText.text = Hp.Value.ToString();

        ogHp = Hp.Value;
        _enemySystem = FindObjectOfType<EnemySystem>();

    }

    private void OnDamageTaken(int previousValue, int newValue)
    {
        hitted = true;
        //hpText.text = Hp.Value.ToString();

        sliderFill.fillAmount = newValue / ogHp;

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
