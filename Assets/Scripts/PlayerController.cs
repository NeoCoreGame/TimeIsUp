using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;
using static Unity.Burst.Intrinsics.X86;
using Unity.Collections;
using UnityEngine.SceneManagement;
using Unity.Netcode.Components;

public class PlayerController : NetworkBehaviour, IShootable
{
    //PRIVATE
    private CharacterController _cController;

    private Vector3 _movement;
    private Vector3 _gravMovement;
    private Vector2 recievedInput;

    private PlayerCountdown _playerCountdown;
    private HealthController _healthController;

    //PUBLIC
    [Header("Movement Parameters")]
    public float gravity;
    public NetworkVariable<bool> canMove = new NetworkVariable<bool>();

    public float _speed;
    public float _fallingSpeed;

    [Header("PlayerUI")]
    public TextMeshPro _playerNumber;

    [Header("Ground Check Parameters")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    private RespawnPlayerManager _respawnPlayerManager;



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _cController = GetComponent<CharacterController>();
        _playerCountdown = GetComponent<PlayerCountdown>();
        _healthController = GetComponent<HealthController>();
        _respawnPlayerManager = FindObjectOfType<RespawnPlayerManager>();

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
        }
        
        _gravMovement = new Vector3(0f,gravity,0f);
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {

            if (canMove.Value)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                _movement = transform.right * recievedInput.x + transform.forward * recievedInput.y;
                _cController.Move(_movement * _speed * Time.deltaTime);
            }
            if (!isGrounded)
                {
                    _cController.Move(_gravMovement * _fallingSpeed * Time.deltaTime);
            }
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

    public override void OnDestroy()
    {
        //Limpio al jugador
        ServerGameManager.Instance.GetCoundownManager().contadores.Remove(_playerCountdown);


        //Funcion base
        base.OnDestroy();
    }
    public void TakeDamage(int dmg)
    {

        OnTakeDamageServerRpc(dmg);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnTakeDamageServerRpc(int dmgTaken)
    {
        _healthController.HP.Value -= dmgTaken;

        if (_healthController.HP.Value <= 0)
        {
            //GetComponent<NetworkTransform>().Teleport(new Vector3(-45f, 9f, 26f),transform.rotation,transform.localScale);
            _playerCountdown.TimeVariation(-60);
            // _healthController.HP.Value = 100;
            _respawnPlayerManager.RespawnPlayer(_healthController);

        }
    }   

    public int GetHealth()
    {
        return _healthController.HP.Value;
    }

    public float GetTimeReward()
    {
        return 100f;
    }

    public CharacterController GetController()
    {
        return _cController;
    }
}
