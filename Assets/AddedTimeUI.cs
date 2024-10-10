using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddedTimeUI : MonoBehaviour
{
    public bool showing;

    private Vector3 reference;
    private Vector3 _destinyScale = new Vector3(1.5f, 1.5f, 1.5f);

    public float speed;

    private TextMeshProUGUI UI;

    private float offset = .1f;

    private float transitionTimer;

    public int shownNumber;

    private void Start()
    {
        UI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(showing)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, _destinyScale, ref reference, Time.deltaTime * speed);

            if(transform.localScale.x > 1.5 - offset) { transitionTimer += Time.deltaTime; }
            if(transitionTimer >= 1f) { showing = false; }
        }
        else
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref reference, Time.deltaTime * (speed*8));
        }
    }
    
    public void LaunchText(float addedTime)
    {
        if(addedTime > 0)
        {
            UI.color = Color.green;
            UI.text = "+" + addedTime.ToString();
        }
        else
        {
            UI.color = Color.red;
            UI.text = "-" + addedTime.ToString();
        }
        transitionTimer = 0;

        showing = true;
    }
}
