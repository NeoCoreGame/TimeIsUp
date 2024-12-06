using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private ExplosivoBehaviour m_Minion;

    private void Start()
    {
        m_Minion = transform.parent.GetComponent<ExplosivoBehaviour>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.GetComponent<PlayerController>())
        {
            m_Minion.explotarJugador = true;
        }
    }

}
