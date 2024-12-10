using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private TanqueBehaviourTres m_Minion;

    public enum attackType
    {
        Close,
        Far
    }

    public attackType type;

    private void Start()
    {
        m_Minion = transform.parent.GetComponent<TanqueBehaviourTres>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (type == attackType.Close)
        {
            //Debug.Log(other.gameObject);
            if (other.GetComponent<PlayerController>())
            {

                m_Minion.atacarPlayerFar = true;
            } 
        }
        else
        {
            //Debug.Log(other.gameObject);
            if (other.GetComponent<PlayerController>())
            {
                m_Minion.atacarPlayerFar = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type == attackType.Close)
        {
            //Debug.Log(other.gameObject);
            if (other.GetComponent<PlayerController>())
            {
                m_Minion.atacarPlayerFar = false;
            }
        }
        else
        {
            //Debug.Log(other.gameObject);
            if (other.GetComponent<PlayerController>())
            {
                m_Minion.atacarPlayerFar = false;
            }
        }
    }
}
