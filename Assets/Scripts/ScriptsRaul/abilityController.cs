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

    public GameObject skIcon;
    public GameObject tdIcon;
    public GameObject petuniaIcon;

    private void Start()
    {
        cam = Camera.main.transform;

        //bombBackIcon = GameObject.Find("bombAbilityIcon");
        //hookBackIcon = GameObject.Find("hookAbilityIcon");
        //stunBackIcon = GameObject.Find("stunAbilityIcon");
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
                //hookBackIcon.SetActive(false);
                //stunBackIcon.SetActive(false);
                //GameObject.Find("hookAbilityGrayIcon").SetActive(false);
                //GameObject.Find("stunAbilityGrayIcon").SetActive(false);
                tdIcon.SetActive(false);
                petuniaIcon.SetActive(false);

                break;
            case 1:
                //Hook Ability
                hookAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
                hookAbility.icon.fillAmount = 0;
                hookAbility.cooldown = 5;
                hookAbility.isOnCooldown = false;
                //bombBackIcon.SetActive(false);
                //stunBackIcon.SetActive(false);
                //GameObject.Find("bombAbilityGrayIcon").SetActive(false);
                //GameObject.Find("stunAbilityGrayIcon").SetActive(false);
                skIcon.SetActive(false);
                petuniaIcon.SetActive(false);
                break;
            case 2:
                //Stun Ability
                stunAbility.icon = GameObject.Find("stunAbilityGrayIcon").GetComponent<Image>();
                stunAbility.icon.fillAmount = 0;
                stunAbility.cooldown = 2;
                stunAbility.isOnCooldown = false;
                //hookBackIcon.SetActive(false);
                //bombBackIcon.SetActive(false);
                //GameObject.Find("hookAbilityGrayIcon").SetActive(false);
                //GameObject.Find("bombAbilityGrayIcon").SetActive(false);
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
                    BombAbility();
                }
                bombAbility.AbilityInput();
                bombAbility.AbilityCooldown();
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Q) && !hookAbility.isOnCooldown)
                {
                    HookAbility();
                }
                hookAbility.AbilityInput();
                hookAbility.AbilityCooldown();
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Q) && !stunAbility.isOnCooldown)
                {
                    StunAbility();
                }
                stunAbility.AbilityInput();
                stunAbility.AbilityCooldown();
                break;
        }
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

        var stunInstance = Instantiate(stunObject, granny.transform.position + (new Vector3(0, -1, 0)) + granny.transform.forward * 10, granny.transform.rotation);
    }

}
