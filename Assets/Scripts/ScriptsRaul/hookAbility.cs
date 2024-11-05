using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class hookAbility : Ability
{

    public string[] tagsToCheck;
    public float speed;
    public float returnSpeed;
    public float range;
    public float stopRange;

    [HideInInspector]
    public Transform caster;
    [HideInInspector]
    public Transform collidedWith;
    private LineRenderer line;
    private bool collided;


    private void Start()
    {
        line = transform.Find("Line").GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(caster)
        {
            line.SetPosition(0, caster.position);
            line.SetPosition(1, transform.position);

            var dist = Vector3.Distance(transform.position, caster.position);
            if (collided)
            {
                transform.LookAt(caster);

                if (dist < stopRange)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                
                if (dist > range)
                {
                    Collision(null);
                }
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if(collidedWith)
            {
                collidedWith.transform.position = transform.position;
            }

        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!collided && tagsToCheck.Contains(other.tag))
        {
            Collision(other.transform.parent);
        }
    }

    void Collision(Transform collision)
    {
        speed = returnSpeed;
        collided = true;

        if(collision)
        {
            transform.position = collision.position;
            collidedWith = collision;
        }
    }

}

