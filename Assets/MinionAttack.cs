using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : MonoBehaviour
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
               // m_Minion.
            }
        
    }
}
