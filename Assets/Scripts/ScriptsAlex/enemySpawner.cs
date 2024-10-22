using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class enemySpawner : MonoBehaviour
{
    private GameObject enemy;
    public GameObject enemy3;
    private int enemyCount;
    private int enemy2Count;


    private int randomSpawn;
    public GameObject[] spawnPoint;

    public EnemySystem _enemySystem;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        enemy2Count = 0;
        StartCoroutine(enemySpawn());

        _enemySystem = FindObjectOfType<EnemySystem>();
    }

    private void spawnEnemy1(int spawn)
    {
        enemy = enemyPool.instance.GetPooledEnemy();

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint[spawn].transform.position;
            enemy.SetActive(true);

            //GameObject child = enemy.transform.GetChild(0).transform.gameObject;
            //_enemySystem.EnemyStartupHealth(child, -100);

            enemyCount++;
        }
    }

    private void spawnEnemy2(int spawn)
    {
        enemy = enemy2Pool.instance.GetPooledEnemy();

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint[spawn].transform.position;
            enemy.SetActive(true);
            enemy2Count++;
        }
    }

    IEnumerator enemySpawn()
    {
        while(enemyCount < 200)
        {
            randomSpawn = Random.Range(0, 5);
            spawnEnemy1(randomSpawn);
            randomSpawn = Random.Range(0, 5);
            spawnEnemy1(randomSpawn);

            if (enemy2Count < 40)
            {
                if (enemyCount > 70)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        randomSpawn = Random.Range(0, 5);
                        spawnEnemy2(randomSpawn);
                        enemy2Count++;
                    }
                }
                else if (enemyCount > 50)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        randomSpawn = Random.Range(0, 5);
                        spawnEnemy2(randomSpawn);
                        enemy2Count++;
                    }
                }
                else if (enemyCount > 20)
                {
                    randomSpawn = Random.Range(0, 5);
                    spawnEnemy2(randomSpawn);
                    enemy2Count++;
                }
            }

            if (enemy2Count > 20)
            {
                enemy2Count = 0;
                randomSpawn = Random.Range(0, 5);
                Vector3 position = spawnPoint[randomSpawn].transform.position;
                Instantiate(enemy3, position, Quaternion.identity);
            }

            yield return new WaitForSeconds(2f);
        }
        
    }

}
