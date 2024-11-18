using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class abilityController : MonoBehaviour
{
    public Transform cam;
    public GameObject granny;

    public GameObject hookObject;
    public Ability hookAbility;

    public GameObject bombObject;
    public Ability bombAbility;

    public GameObject stunObject;
    public Ability stunAbility;

    private void Start()
    {
        cam = Camera.main.transform;

        //Hook Ability
        hookAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
        hookAbility.icon.fillAmount = 0;
        hookAbility.cooldown = 5;
        hookAbility.isOnCooldown = false;

        //Bomb Ability
        bombAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
        bombAbility.icon.fillAmount = 0;
        bombAbility.cooldown = 5;
        bombAbility.isOnCooldown = false;

        //Stun Ability
        stunAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
        stunAbility.icon.fillAmount = 0;
        stunAbility.cooldown = 2;
        stunAbility.isOnCooldown = false;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q) && !hookAbility.isOnCooldown)
        //{
        //    HookAbility();
        //}
        //hookAbility.AbilityInput();
        //hookAbility.AbilityCooldown();

        //if (Input.GetKeyDown(KeyCode.Q) && !bombAbility.isOnCooldown)
        //{
        //    BombAbility();
        //}
        //bombAbility.AbilityInput();
        //bombAbility.AbilityCooldown();

        if (Input.GetKeyDown(KeyCode.Q) && !stunAbility.isOnCooldown)
        {
            StunAbility();
        }
        stunAbility.AbilityInput();
        stunAbility.AbilityCooldown();
    }


    void HookAbility()
    {
        Debug.Log("Q apretada");
        var hook = Instantiate(hookObject, granny.transform.position + granny.transform.forward, granny.transform.rotation);
        hook.GetComponent<hookAbility>().caster = transform;
    }

    void BombAbility()
    {
        Debug.Log("Q apretada");

        var bombInstance = Instantiate(bombObject, granny.transform.position + granny.transform.forward, granny.transform.rotation);

        Rigidbody bombInstanceRb = bombInstance.GetComponent<Rigidbody>();

        Vector3 forceToAdd = granny.transform.transform.forward * bombInstance.GetComponent<bombAbility>().throwForce + transform.up * bombInstance.GetComponent<bombAbility>().throwUpwardForce;

        bombInstanceRb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    void StunAbility()
    {
        Debug.Log("Q apretada Stun Ability");

        var stunInstance = Instantiate(stunObject, granny.transform.position + (new Vector3(0, 0, 0)) + granny.transform.forward * 5, granny.transform.rotation);
    }

}
