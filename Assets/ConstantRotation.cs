using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float degrees;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, -degrees * Time.deltaTime, 0f));
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
    }
}
