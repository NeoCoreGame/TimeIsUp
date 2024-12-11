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
using Unity.Collections.LowLevel.Unsafe;
using static UnityEngine.Rendering.DebugUI;

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

    public NetworkVariable<int> selectedCharacter = new NetworkVariable<int>();
    public GameObject[] characters;

    private InitialCountdown iC;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            OnCharacterChangeServerRpc(4);

        }

        _cController = GetComponent<CharacterController>();
        _playerCountdown = GetComponent<PlayerCountdown>();
        _healthController = GetComponent<HealthController>();
        _respawnPlayerManager = FindObjectOfType<RespawnPlayerManager>();

        Invoke("FixerN", 1f);

        if (IsOwner)
        {

            Invoke("SetupGame", 2f);
        }

        Invoke("ChangeCharacters", 3f);

        _gravMovement = new Vector3(0f,gravity,0f);
    }
    public void FixerN()
    {

        selectedCharacter.OnValueChanged += OnCharacterChange;
    }

    public void SetupGame()
    {
        iC = FindObjectOfType<InitialCountdown>();
        iC.StartCounter();

        GetComponent<PlayerInput>().enabled = true;
        GetComponent<abilityController>().enabled = true;
        OnCharacterChangeServerRpc(StaticData.characterID);

        Invoke("SetCameraToCharacter", 3f);
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

    public void SetCameraToCharacter()
    {
        GetComponent<MouseLook>().SetCamera(this, characters[selectedCharacter.Value].transform.GetChild(2).transform.position);
        
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

    public void TeleportPlayer(Vector3 newPosition)
    {
        OnTeleportServerRpc(newPosition);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnTeleportServerRpc(Vector3 newPosition)
    {
        _cController.enabled = false;
        transform.position = newPosition;
        _cController.enabled = true;

        //_movement = new Vector3(aux.x, 0f, aux.y);
    }

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
    public void OnCharacterChange(int previousValue, int newValue)
    {
        foreach (GameObject c in characters) { c.SetActive(false); }
        characters[newValue].SetActive(true);
        if (IsOwner)
        {
            Invoke("RidOfAnim", 2f);
            characters[newValue].transform.GetChild(1).gameObject.layer = 10;
            characters[newValue].transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject.layer = 9;

            if (newValue == 2)
            {


                characters[newValue].transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).gameObject.layer = 10;
            }
            GetComponent<ShootingController>().SetAnim();
        }

    }

    public void RidOfAnim()
    {
      transform.GetChild(0).GetComponent<Animator>().enabled = false;
    }

    public void ChangeCharacters()
    {
        foreach (GameObject c in characters) { c.SetActive(false); }
        characters[selectedCharacter.Value].SetActive(true);
        //characters[selectedCharacter.Value].GetComponent<Animator>().enabled = true;
    }


    [ServerRpc(RequireOwnership = false)]
    public void OnCharacterChangeServerRpc(int value)
    {
        selectedCharacter.Value = value;
        Debug.Log(value);
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

    public void Stun(float time)
    {
        throw new System.NotImplementedException();
    }
}
