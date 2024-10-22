using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class enemy2Pool : MonoBehaviour
{
    public static enemy2Pool instance;

    private List<GameObject> pooledEnemies = new List<GameObject>();
    private int enemiesToPool = 10;

    [SerializeField] private GameObject enemy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void InitializePool(GameObject enemy, EnemySystem _enemySystem)
    {
            //GameObject obj = Instantiate(enemy);
            //obj.transform.GetChild(0).GetComponent<Enemy>().InitializeEnemy();
            //obj.SetActive(false);
            //pooledEnemies.Add(obj);
            //obj.GetComponent<NetworkObject>().Spawn();
        
    }

    public GameObject GetPooledEnemy()
    {
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }

        return null;
    }

}
