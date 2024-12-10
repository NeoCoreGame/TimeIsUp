using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class enemyPool : MonoBehaviour
{
    public static enemyPool instance;

    private List<GameObject> pooledEnemies = new List<GameObject>();
    private int enemiesToPool = 50;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void InitializePool(GameObject obj)
    {
        obj.GetComponent<Enemy>().InitializeEnemy();
        obj.SetActive(false);
        pooledEnemies.Add(obj);

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
