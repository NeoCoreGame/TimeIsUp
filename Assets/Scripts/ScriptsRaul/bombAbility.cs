using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class bombAbility : Ability
{
    public Transform cam;
    public GameObject bomb;
    public abilityController _player;

    public float throwForce;
    public float throwUpwardForce;
    public float radius;
    public int dmg;

    public AudioClip bombSplashClip;
    public AudioClip prueba;

    public bool thrown;

    private Rigidbody rb;
    private BoxCollider bc;

    private void Start()
    {
        bomb = gameObject;
        _player = FindObjectOfType<abilityController>();
        rb = GetComponent<Rigidbody>();
    }
    public void ThrowBomb()
    {
        thrown = true;
    }

    void OnCollisionEnter(Collision collision)
    {
         if (collision.gameObject.tag == "Map" || collision.gameObject.tag == "Enemy")
            {
                EnemiesHit();
                //SFXManager.instance.PlaySFX(prueba, transform, 1f);
                SFXManager.instance.PlaySFX(bombSplashClip, transform);
                thrown = false;
                transform.parent = _player.transform;
            transform.position = new Vector3(0f, -15.39f, 0f);
            rb.useGravity = false;
            rb.isKinematic = true;
            
        } 
        
    }

    void EnemiesHit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                //QUITAR VIDA AQUI!!!
             //   Debug.Log("EnemyHit");
                if (collider.gameObject.TryGetComponent(out IShootable interactObj))
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
