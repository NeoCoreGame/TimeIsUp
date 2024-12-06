using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionColliderEnemy : MonoBehaviour
{
    private MinionBehaviour m_Minion;

    private void Start()
    {
        m_Minion = transform.parent.GetComponent<MinionBehaviour>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.GetComponent<PlayerController>())
        {
            m_Minion.jugadorVisto = true;
            m_Minion._player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.GetComponent<PlayerController>())
        {
            m_Minion.jugadorVisto = false;
           // m_Minion._player = null;
        }
    }

}
