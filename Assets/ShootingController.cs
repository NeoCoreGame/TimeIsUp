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


    // Start is called before the first frame update
    void Start()
    {
        _playerCountdown = GetComponent<PlayerCountdown>();

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

    public void Shoot()
    {
        Debug.Log("Disparo");
        pS.Play();
        shootingCdTimer = shootingCooldown;



    }

    #region Input
    public void OnShoot(InputAction.CallbackContext context)
    {

        float isShooting = context.ReadValue<float>();
        if (isShooting == 1 && shootingCdTimer <= 0f)
        {
            Shoot();

            Ray rI = new Ray(playerCam.transform.position, playerCam.transform.forward);

            if (Physics.Raycast(rI, out RaycastHit hitInfo, shootingRange, enemyLayer))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IShootable interactObj))
                {
                    Debug.Log("Au");
                    interactObj.TakeDamage(WeaponDmg);

                    if (interactObj.GetHealth() <= 0)
                    {
                        _playerCountdown.TimeVariation(interactObj.GetTimeReward());
                    }
                }

            }
        }

        OnShootServerRpc(context.ReadValue<float>());

    }

    [ServerRpc]
    public void OnShootServerRpc(float input)
    {

        clientShot.Value = input;

    }
    #endregion

}
