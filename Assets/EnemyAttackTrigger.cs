using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private TanqueBehaviour m_Minion;

    public enum attackType
    {
        Close,
        Far
    }

    public attackType type;

    private void Start()
    {
        m_Minion = transform.parent.GetComponent<TanqueBehaviour>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (type == attackType.Close)
        {
            //Debug.Log(other.gameObject);
            if (other.GetComponent<PlayerController>())
            {
                m_Minion.atacarPlayerFar = false;
                m_Minion.atacarPlayerClose = true;
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
}
