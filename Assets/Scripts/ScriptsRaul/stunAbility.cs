using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class stunAbility : Ability
{
    public Transform cam;
    public abilityController _player;
    private float stunDuration = 5;
    private bool isStuned = false;
    private float currentStunTime;

    private List<GameObject> stunedEnemies = new List<GameObject>();

    public Animator _animator;

    public int dmg = 50;

    void Start()
    {
        _player = FindObjectOfType<abilityController>();

    }
    private void OnEnable()
    {
        ThrowSkill();
    }

    private void Update()
    {
        StunTime();
    }

    public void ThrowSkill()
    {
        if (!isStuned)
        {
            _animator.SetTrigger("Up");
            isStuned = true;
            currentStunTime = stunDuration; 
        }
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
                      //  Debug.Log("EnemyMoves");
                    }
                }
                stunedEnemies.Clear();
                transform.parent = _player.transform;
                transform.position = new Vector3(0f, -14.171f, 0f);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !stunedEnemies.Contains(other.gameObject))
        {
            Debug.Log("PILLADO ENEMIGO");
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
                //QUITAR VIDA AQUI!!!
                //   Debug.Log("EnemyHit");
                if (enemy.gameObject.TryGetComponent(out IShootable interactObj))
                {
                    if (interactObj.GetHealth() <= dmg)
                    {
                        //_playerCountdown.TimeVariation(interactObj.GetTimeReward());
                    }
                    interactObj.TakeDamage(dmg);


                }
            }
        }
    }
}
