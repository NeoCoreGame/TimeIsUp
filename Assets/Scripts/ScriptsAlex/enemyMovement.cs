using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class enemyBehaviour : MonoBehaviour
{
    //public GameObject objective;

    public Transform position;

    private Vector3 initialPosition;
    private Vector3 finalPosition;

    public float speed;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    private NavMeshAgent navMeshAgent;
    private Transform[] destinies;

    private float arrivingOffset = 6f;

    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
        groundCheck = transform.GetChild(2);
        navMeshAgent = GetComponent<NavMeshAgent>();
        speed = 3f;



        navMeshAgent.speed = speed;

        destinies = FindObjectOfType<Destinies>().desinyGroup;

        finalPosition = destinies[Random.Range(0, destinies.Length)].position;
        navMeshAgent.SetDestination(finalPosition);

    }

    // Update is called once per frame
    void Update()
    {

        navMeshAgent.SetDestination(finalPosition);
        

        if (HasArrived())
        {
            
            finalPosition = destinies[Random.Range(0, destinies.Length)].position;

            navMeshAgent.SetDestination(finalPosition);
        }


         
        
    }

    public bool HasArrived()
    {
        if (Vector3.Distance(finalPosition,transform.position) < arrivingOffset)
        {
            return true;
        }

        return false;
    }

}
