using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class abilityController : NetworkBehaviour
{
    public Transform cam;
    public GameObject granny;

    public GameObject hookObject;
    public Ability hookAbility;

    public GameObject bombObject;
    public Ability bombAbility;
    public bombAbility bAB;

    public GameObject stunObject;
    public Ability _stunAbility;
    public stunAbility sAB;

    public GameObject skIcon;
    public GameObject tdIcon;
    public GameObject petuniaIcon;

    GameObject hook;
    private float mouseSensitivityAC;

    //SFX
    public AudioClip bombThrowClip;
    public AudioClip hookClip;
    public AudioClip rootsClip;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { enabled = false; }
        base.OnNetworkSpawn();
    }

    private void Start()
    {

        cam = Camera.main.transform;

        skIcon = GameObject.Find("Character1Icons");
        tdIcon = GameObject.Find("Character2Icons");
        petuniaIcon = GameObject.Find("Character3Icons");

        switch (StaticData.characterID)
        {
            case 0:
                //Bomb Ability
                bombAbility.icon = GameObject.Find("bombAbilityGrayIcon").GetComponent<Image>();
                bombAbility.icon.fillAmount = 0;
                bombAbility.cooldown = 5;
                bombAbility.isOnCooldown = false;
                tdIcon.SetActive(false);
                petuniaIcon.SetActive(false);

                break;
            case 1:
                //Hook Ability
                hookAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
                hookAbility.icon.fillAmount = 0;
                hookAbility.cooldown = 5;
                hookAbility.isOnCooldown = false;
                skIcon.SetActive(false);
                petuniaIcon.SetActive(false);
                break;
            case 2:
                //Stun Ability
                _stunAbility.icon = GameObject.Find("stunAbilityGrayIcon").GetComponent<Image>();
                _stunAbility.icon.fillAmount = 0;
                _stunAbility.cooldown = 6;
                _stunAbility.isOnCooldown = false;
                tdIcon.SetActive(false);
                skIcon.SetActive(false);
                break;
        }





    }

    private void Update()
    {
        switch (StaticData.characterID)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Q) && !bombAbility.isOnCooldown)
                {
                    // BombAbility(); 
                    BombAbilityServerRPC();
                    SFXManager.instance.PlaySFX(bombThrowClip, transform);
                }
                bombAbility.AbilityInput();
                bombAbility.AbilityCooldown();
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Q) && !hookAbility.isOnCooldown)
                {
                    HookAbility();
                    SFXManager.instance.PlaySFX(hookClip, transform);
                }
                hookAbility.AbilityInput();
                hookAbility.AbilityCooldown();

                if (!hook)
                {
                    granny.GetComponent<PlayerController>()._speed = 20;
                    granny.GetComponent<MouseLook>().enabled = true;
                    granny.GetComponent<ShootingController>().enabled = true;
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Q) && !_stunAbility.isOnCooldown)
                {
                    //StunAbility();
                    StunAbilityServerRPC();
                    SFXManager.instance.PlaySFX(rootsClip, transform);
                }

                _stunAbility.AbilityInput();
                _stunAbility.AbilityCooldown();
                break;
        }
    }


    void HookAbility()
    {
        granny.GetComponent<PlayerController>()._speed = 0;
        granny.GetComponent<MouseLook>().enabled = false;
        granny.GetComponent<ShootingController>().enabled = false;
        Debug.Log("Q apretada");
        hook = Instantiate(hookObject, granny.transform.position + granny.transform.forward, granny.transform.rotation);
        hook.GetComponent<hookAbility>().caster = transform;
    }

    void BombAbility()
    {
        if (!bAB.thrown)
        {
            Debug.Log("Q apretada");
            Rigidbody bombInstanceRb = bombAbility.GetComponent<Rigidbody>();
            // var bombInstance = Instantiate(bombObject, granny.transform.position + granny.transform.forward, granny.transform.rotation);

            bombInstanceRb.useGravity = false;
            bombInstanceRb.isKinematic = true;
            bombAbility.transform.localPosition = granny.transform.position + granny.transform.forward;
            bombAbility.transform.parent = null;
            bombAbility.transform.localPosition = granny.transform.position + granny.transform.forward;

            bombInstanceRb.useGravity = true;
            bombInstanceRb.isKinematic = false;


            Vector3 forceToAdd = granny.transform.transform.forward * bombAbility.GetComponent<bombAbility>().throwForce + transform.up * bombAbility.GetComponent<bombAbility>().throwUpwardForce;

            bombInstanceRb.AddForce(forceToAdd, ForceMode.Impulse);

            bAB.ThrowBomb();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void BombAbilityServerRPC()
    {
        BombAbility();
    }

    [ServerRpc(RequireOwnership = false)]
    void StunAbilityServerRPC()
    {
        StunAbility();
    }

    void StunAbility()
    {
        Debug.Log("Q apretada Stun Ability");

        //var stunInstance = Instantiate(stunObject, granny.transform.position + (new Vector3(0, -1, 0)) + granny.transform.forward * 10, granny.transform.rotation);
        _stunAbility.transform.parent = null;
        _stunAbility.transform.localPosition = granny.transform.position + (new Vector3(0, -1, 0)) + granny.transform.forward * 10;
        sAB.ThrowSkill();
    }

}
