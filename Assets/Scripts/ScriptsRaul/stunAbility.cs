using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class stunAbility : Ability
{
    public Transform cam;
    public GameObject roots;
    private float stunDuration = 5;
    private bool isStuned = false;
    private float currentStunTime;

    private List<GameObject> stunedEnemies = new List<GameObject>();

    void Start()
    {
        roots = gameObject;
    }

    private void Update()
    {
        StunTime();
    }

    public void StunTime()
    {
        if (isStuned)
        {

            currentStunTime -= Time.deltaTime;

            if (currentStunTime <= 0f)
            {
                isStuned = false;
                currentStunTime = 0f;
                foreach (GameObject enemy in stunedEnemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        //VOLVER A MOVER ENEMIGOS AQUI!!!
                        Debug.Log("EnemyMoves");
                    }
                }
                stunedEnemies.Clear();
                Destroy(roots);
            }
        }
        else
        {
            isStuned = true;
            currentStunTime = stunDuration;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !stunedEnemies.Contains(other.gameObject))
        {
            stunedEnemies.Add(other.gameObject);
            EnemiesHit();
        }
    }

    private void EnemiesHit()
    {
        foreach (GameObject enemy in stunedEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                //FRENAR ENEMIGOS AQUI!!!
                Debug.Log("EnemyStunned");
            }
        }
    }
}
