using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour : MonoBehaviour
{
    //public GameObject objective;

    public Transform position;

    private Vector3 initialPosition;
    private Vector3 finalPosition;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
        speed = 10f;

        initialPosition = position.position;

        finalPosition = new Vector3(Random.Range(0f, 50f), initialPosition.y , Random.Range(0f, 50f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, speed * Time.deltaTime);
        if (transform.position == finalPosition)
        {
            initialPosition = finalPosition;
            finalPosition = new Vector3(Random.Range(0f, 50f), initialPosition.y ,Random.Range(0f, 50f));
        }
    }

}
