using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilty : MonoBehaviour
{

    public float cooldown;
    private float currentCooldown;
    private Image icon;
    private bool isOnCooldown = false;

    private void Start()
    {
        icon = GameObject.Find("IconGray").GetComponent<Image>();
        icon.fillAmount = 0;
    }

    private void Update()
    {

        AbilityInput();
        AbilityCooldown();

    }

    public void AbilityInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
        {
            Debug.Log("space pressed");
            isOnCooldown=true;
            currentCooldown = cooldown;
        }
    }

    public void AbilityCooldown()
    {
        if (isOnCooldown)
        {

            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                isOnCooldown = false;
                currentCooldown = 0f;

                if(icon != null)
                {
                    icon.fillAmount = 0;
                }
            }
            else
            {
                if (icon != null)
                {
                    Debug.Log(currentCooldown);
                    Debug.Log(cooldown);
                    Debug.Log(currentCooldown / cooldown);
                    icon.fillAmount = currentCooldown / cooldown;
                }
            }

        }
        
    }
}
