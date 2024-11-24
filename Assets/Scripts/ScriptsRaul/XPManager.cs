using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{

    public Image xpBar;
    public Image clockLVL;

    private float excessXP;

    // Start is called before the first frame update
    void Start()
    {
        xpBar = GameObject.Find("XPBarFill").GetComponent<Image>();
        clockLVL = GameObject.Find("ClockFill").GetComponent<Image>();
    }

    public void GainXP(float xp)
    {
        if ((xpBar.fillAmount + xp) >= 1f)
        {
            excessXP = (xpBar.fillAmount + xp) - 1f; Debug.Log(excessXP);
            xpBar.fillAmount = excessXP;
            clockLVL.fillAmount += 0.01f;
        }
        else
        {
            xpBar.fillAmount += xp;
        }
    }
}
