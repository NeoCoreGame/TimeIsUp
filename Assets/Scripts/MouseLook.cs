using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : NetworkBehaviour
{

    [Range(0f, 4f)]
    public float mouseSensitivity;
    public Transform head;
    public Transform player;
    public float xRotation = 0f;

    public float mouseX;
    public float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        if (IsOwner)
        {
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = new Vector3(0f, 0.6f, 0.5f);

            Cursor.lockState = CursorLockMode.Locked;
            head = transform;
            player = transform.parent.transform.parent.transform;
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //No la multiplico por Time.DeltaTime para que no dependa del FrameRate
        if (IsServer)
        {
            player.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);



        }
    }

    #region Input
    public void OnLook(InputAction.CallbackContext context)
    {
        OnLookServerRpc(context.ReadValue<Vector2>());
    }

    [ServerRpc]
    public void OnLookServerRpc(Vector2 input)
    {
        mouseX = input.x * mouseSensitivity;
        mouseY = input.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Debug.Log("Intentando mirar " + mouseX + " , " + mouseY + " , " + xRotation);

    }
    #endregion

}
