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

      //  head = playerController.heads[playerController.selectedCharacter.Value].transform;
    }

    public void SetCamera(PlayerController c, Vector3 pos)
    {
        playerController = c;
        head = playerController.heads[playerController.selectedCharacter.Value].transform;
        Camera.main.transform.localPosition = pos;
        Camera.main.transform.parent = head.transform;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //No la multiplico por Time.DeltaTime para que no dependa del FrameRate
        if (NetworkManager.Singleton.IsServer)
        {
           // player.Rotate(Vector3.up * mouseX);
            //head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //head.Rotate(Vector3.right * xRotation);

        }
    }

    public void UpdateMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }

    #region Input
    public void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>()); 
        if (IsOwner)
        {

            OnLookServerRpc(context.ReadValue<Vector2>()); 
        }


    }

    [ServerRpc(RequireOwnership = false)]
    public void OnLookServerRpc(Vector2 input)
    {
        if (playerController != null &&playerController.canMove.Value)
        {
            mouseX = input.x * mouseSensitivity;
            mouseY = input.y * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            player.Rotate(Vector3.up * mouseX);
            head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

    }
    #endregion

}
