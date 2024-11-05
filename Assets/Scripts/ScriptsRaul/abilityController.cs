using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityController : MonoBehaviour
{
    public Transform cam;
    public GameObject hookObject;
    public Ability hookAbility;

    private void Start()
    {
        cam = Camera.main.transform;
        hookAbility.icon = GameObject.Find("hookAbilityGrayIcon").GetComponent<Image>();
        hookAbility.icon.fillAmount = 0;
        hookAbility.cooldown = 5;
        hookAbility.isOnCooldown = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !hookAbility.isOnCooldown)
        {
            HookAbility();
        }
        hookAbility.AbilityInput();
        hookAbility.AbilityCooldown();
    }


    void HookAbility()
    {
        Debug.Log("Q apretada");
        var hook = Instantiate(hookObject, cam.position + cam.forward, cam.rotation);
        hook.GetComponent<hookAbility>().caster = transform;
    }

}
