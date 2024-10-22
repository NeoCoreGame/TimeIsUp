using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySystem : NetworkBehaviour
{
    enemyPool _enemyPool;
    enemy2Pool _enemy2Pool;

    public GameObject enemyContainer;
     List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        for(int i = 0; i< enemyContainer.transform.childCount; i++)
        {
            enemies.Add(enemyContainer.transform.GetChild(i).gameObject);
        }
    }


    public void InitializePools()
    {
        _enemyPool = FindObjectOfType<enemyPool>();
        _enemy2Pool = FindObjectOfType<enemy2Pool>();

        foreach(GameObject e in enemies)
        {

            _enemyPool.InitializePool(e);

        }
       //_enemy2Pool.InitializePool(_enemyPrefab, this);

    }
    

    public void EnemyTakeDamage(GameObject enemy, int damage)
    {
        GameObject parent = enemy.transform.parent.gameObject;
        OnTakeDamageServerRpc(enemies.IndexOf(parent), damage);
    }
    public void EnemyStartupHealth(GameObject enemy, int damage)
    {
        if (enemy.GetComponent<Enemy>().Hp.Value <= 0)
        {
            GameObject parent = enemy.transform.parent.gameObject;
            OnTakeDamageServerRpc(enemies.IndexOf(parent), damage); 
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnTakeDamageServerRpc(int enemyIndex, int dmgTaken)
    {
        
            Enemy e = enemies[enemyIndex].transform.GetChild(0).transform.GetComponent<Enemy>();
            e.Hp.Value -= dmgTaken;

            if (e.Hp.Value <= 0)
            {
                GameObject parent = e.transform.parent.gameObject;
                parent.SetActive(false);
                e.Hp.Value = 100;
        }
        
    }
    
}