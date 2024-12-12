using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public GameObject arrowLeft;
    public GameObject arrowRight;

    public float advance;

    public RectTransform viewportContent;

    public float leftBound;
    public float rightBound;

    // Update is called once per frame
    void Update()
    {
        if (viewportContent.offsetMin.x > leftBound)
        {
            arrowLeft.GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        }
        else
        {
            arrowLeft.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        if (viewportContent.offsetMin.x < rightBound)
        {
            arrowRight.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            arrowRight.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void advanceLeft()
    {
        viewportContent.offsetMin += new Vector2(1f, 0f) * advance;
    }

    public void advanceRight()
    {
        viewportContent.offsetMin -= new Vector2(1f, 0f) * advance;
    }
}
