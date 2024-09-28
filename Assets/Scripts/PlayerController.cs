using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;
using static Unity.Burst.Intrinsics.X86;

public class PlayerController : NetworkBehaviour
{
    CharacterController _cController;

    Vector3 _movement;
    Vector2 recievedInput;
    Transform _playerTransform;

    float _speed = 10f;

    public TextMeshPro _playerNumber;


    

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = transform;
        _cController = GetComponent<CharacterController>();

        if(IsOwner) { GetComponent<PlayerInput>().enabled = true; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            _movement = transform.right * recievedInput.x + transform.forward * recievedInput.y;
            _cController.Move(_movement * _speed * Time.deltaTime); 
        }
    }

    #region Input
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveServerRpc(context.ReadValue<Vector2>());
    }

    [ServerRpc]
    public void OnMoveServerRpc(Vector2 input)
    {
        recievedInput = input;
        //_movement = new Vector3(aux.x, 0f, aux.y);
    }
    #endregion

}
