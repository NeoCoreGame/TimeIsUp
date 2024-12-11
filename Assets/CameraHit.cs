using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraHit : MonoBehaviour
{
    //PRIVATE
    private Vector3 og_Pos;
    private Vector3 destiny;

    private Vector3 reference = Vector3.zero;
    private float colorReference = 0f;

    private bool effectPlaying;
    private bool CamHit;

    //PUBLIC
    [Header("Head Drag Parameters")]
    [Range(0.1f,1f)]
    public float dragDistance;
    [Range(0.1f, 1f)]
    public float effectOffset;


    [Header("Red Vignette Parameters")]
    public Image redVignette;
    [Range(0f, 200f)]
    public float vignetteAlphaGoal;


    [Header("Effect Speeds")]
    public float hitSpeed;
    public float colorSpeed;

    public void InitializeScript(Vector3 CamSnapPos)
    {
        og_Pos = CamSnapPos;
        destiny = new Vector3(og_Pos.x, og_Pos.y, -dragDistance);
    }

    public void GetHit()
    {
        effectPlaying = true;
        CamHit = true;
    }

    public void UpdateAlpha(float alpha)
    {
        Color auxColor = redVignette.color;
        auxColor.a = alpha;
        redVignette.color = auxColor;
    }

    private void Update()
    {

        if (effectPlaying)
        {
            if (CamHit)
            {
               // transform.localPosition = Vector3.SmoothDamp(transform.localPosition, destiny, ref reference, hitSpeed * Time.deltaTime);

                float vignetteAlpha = Mathf.SmoothDamp(redVignette.color.a, vignetteAlphaGoal, ref colorReference, colorSpeed *Time.deltaTime);
                UpdateAlpha(vignetteAlpha);

              //  if (transform.localPosition.z < (destiny.z + effectOffset)) { CamHit = false; }

                if(redVignette.color.a >= vignetteAlphaGoal - 10f) { CamHit = false; }
            }
            else
            {
               // if (transform.localPosition.z != og_Pos.z)
                //{
                  //  transform.localPosition = Vector3.SmoothDamp(transform.localPosition, og_Pos, ref reference, hitSpeed/2 * Time.deltaTime);

                    float vignetteAlpha = Mathf.SmoothDamp(redVignette.color.a, 0f, ref colorReference, colorSpeed/2 * Time.deltaTime);
                    UpdateAlpha(vignetteAlpha);


                    if (redVignette.color.a <= 0f) { effectPlaying = false; }

               // }
               // else
               // {
               //     effectPlaying = false;
                //}
            } 
        }
    }
}
