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
    public GameObject hookTip;
    public GameObject player;

    [HideInInspector]
    public Transform caster;
    [HideInInspector]
    public Transform collidedWith;
    private LineRenderer line;

    private bool collided;


    private void Start()
    {
        line = transform.Find("Line").GetComponent<LineRenderer>();
        hookTip.transform.Rotate(0, 180, 0, Space.Self);
        //Vector3 invertedPosition = caster.position * -1f;
        //hookTip.transform.LookAt(caster);

        
    }

    private void Update()
    {
        if(caster)
        {
            line.SetPosition(0, caster.position);
            line.SetPosition(1, transform.position);
            //hookTip.transform.Rotate(0, 0, 180, Space.Self);
            var dist = Vector3.Distance(transform.position, caster.position);
            //var dir = (transform.position - caster.position) / dist;

            //hookTip.transform.LookAt(dir);

            if (collided)
            {
                Transform ogParent = hookTip.transform.parent;
                hookTip.transform.parent = null;
                transform.LookAt(caster);
                hookTip.transform.parent = ogParent;

                
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
        //hookTip.transform.Rotate(0, 180, 0, Space.Self);

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

        //hookTip.transform.Rotate(0,0,180,Space.Self);

        if(collision)
        {
            transform.position = collision.position;
            collidedWith = collision;
        }
    }

}

