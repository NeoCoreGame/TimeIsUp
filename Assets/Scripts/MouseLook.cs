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

    public PlayerController playerController;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }

    public void SetCamera(PlayerController c, Vector3 pos)
    {
        playerController = c;
        head = playerController.characters[playerController.selectedCharacter.Value].transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0);

        Camera.main.transform.localPosition = pos;
        Camera.main.transform.parent = head.transform;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //No la multiplico por Time.DeltaTime para que no dependa del FrameRate
        if (playerController.IsServer)
        {
            player.Rotate(Vector3.up * mouseX);
            //head.Rotate(Vector3.right * xRotation);


        }
    }

    void LateUpdate()
    {
        //No la multiplico por Time.DeltaTime para que no dependa del FrameRate
        if (playerController.IsServer)
        {
            head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //head.Rotate(Vector3.right * xRotation);


        }
    }

    #region Input
    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        OnLookServerRpc(context.ReadValue<Vector2>());
    }

    [ServerRpc]
    public void OnLookServerRpc(Vector2 input)
    {
        if (playerController != null &&playerController.canMove.Value)
        {
            mouseX = input.x * mouseSensitivity;
            mouseY = input.y * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }

    }
    #endregion

}
