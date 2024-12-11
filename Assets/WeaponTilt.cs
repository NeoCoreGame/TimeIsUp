using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTilt : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float tiltMultiplier;


    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //Movement Inputs
        float x = Input.GetAxisRaw("Horizontal") * tiltMultiplier;
        float y = Input.GetAxisRaw("Vertical") * tiltMultiplier;


        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

        Vector3 targetTilt = transform.localRotation * new Vector3(1f, y, -x);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetTilt), smooth * Time.deltaTime);
    }
}
