using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBattlePassXP : MonoBehaviour
{
    public Image xpBar;
    public Image lvlClock;

    public TextMeshProUGUI xpPercentage;
    public TextMeshProUGUI lvlPercent;

    private void Update()
    {
        xpPercentage.text = (xpBar.fillAmount * 100).ToString("F1") + "%";
        lvlPercent.text = "Level " + (lvlClock.fillAmount*100).ToString("F0");
    }
}
