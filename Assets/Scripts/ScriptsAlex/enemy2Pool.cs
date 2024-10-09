using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2Pool : MonoBehaviour
{
    public static enemy2Pool instance;

    private List<GameObject> pooledEnemies = new List<GameObject>();
    private int enemiesToPool = 40;

    [SerializeField] private GameObject enemy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemiesToPool; i++)
        {
            GameObject obj = Instantiate(enemy);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
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
