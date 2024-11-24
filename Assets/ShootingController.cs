using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootingController : NetworkBehaviour
{

    //Shooting params
    public int shootingRange;
    public float shootingCooldown;
    private float shootingCdTimer;
    public LayerMask enemyLayer;
    public int WeaponDmg;

    [SerializeField]
    public NetworkVariable<float> clientShot = new NetworkVariable<float>();

    public ParticleSystem pS;

    private PlayerCountdown _playerCountdown;

    private Camera playerCam;
    private PlayerController playerController;
    public Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        _playerCountdown = GetComponent<PlayerCountdown>();
        playerController = GetComponent<PlayerController>();

        playerCam = Camera.main;


    }

    // Update is called once per frame
    void Update()
    {
        //El cooldown del disparo quizá debería enviarse
        if (shootingCdTimer > 0f) { shootingCdTimer -= Time.deltaTime; }

        if (clientShot.Value == 1 && shootingCdTimer <= 0f && !IsOwner)
        {
            Shoot();
        }

    }

    public void SetAnim()
    {
        _anim = playerController.characters[playerController.selectedCharacter.Value].GetComponent<Animator>();
    }

    public void Shoot()
    {
        _anim.SetTrigger("Attack");
        pS.Play();
        shootingCdTimer = shootingCooldown;



    }

    #region Input
    public void OnShoot(InputAction.CallbackContext context)
    {

        float isShooting = context.ReadValue<float>();

        OnShootServerRpc(context.ReadValue<float>());
        if (isShooting == 1 && shootingCdTimer <= 0f)
        {

            Ray rI = new Ray(playerCam.transform.position, playerCam.transform.forward);

            if (Physics.Raycast(rI, out RaycastHit hitInfo, shootingRange, enemyLayer))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IShootable interactObj))
                {
                    Debug.Log("Au");
    
                    if (interactObj.GetHealth() <= WeaponDmg)
                    {
                        _playerCountdown.TimeVariation(interactObj.GetTimeReward());
                    }
                    interactObj.TakeDamage(WeaponDmg);

                    
                }

            }

            Shoot();
        }


    }

    [ServerRpc]
    public void OnShootServerRpc(float input)
    {

        clientShot.Value = input;

    }
    #endregion

}
