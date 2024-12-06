using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionColliderEnemy : MonoBehaviour
{
    private IEnemyBehaviour m_Minion;

    private void Start()
    {
        m_Minion = transform.parent.GetComponent<IEnemyBehaviour>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.GetComponent<PlayerController>())
        {
            m_Minion.DetectPlayer(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.GetComponent<PlayerController>())
        {
            m_Minion.CleanPlayer();
        }
    }

}
