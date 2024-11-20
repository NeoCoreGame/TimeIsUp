using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private RawImage backgroundImage;
    [SerializeField]
    private float x, y;

    private void Update()
    {
        backgroundImage.uvRect = new Rect(backgroundImage.uvRect.position + new Vector2(x,y) * Time.deltaTime, backgroundImage.uvRect.size);
    }
}
