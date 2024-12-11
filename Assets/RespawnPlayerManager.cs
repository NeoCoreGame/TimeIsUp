using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnPlayerManager : MonoBehaviour
{
    private NetworkManager _networkManager;

    public List<HealthProfile> contadores = new List<HealthProfile>();

    private int startingHP = 100;

    private void Start()
    {
        _networkManager = NetworkManager.Singleton;
    }

    public void StartHealth(HealthController _hC)
    {
        _hC.HP.Value = startingHP;
        HealthProfile healthProfile = new HealthProfile(_hC,_hC.gameObject.GetComponent<PlayerController>());

        contadores.Add(healthProfile);

    }

    public void RespawnPlayer(HealthController hC)
    {
        if (_networkManager.IsServer && contadores.Count > 0)
        {
            foreach (HealthProfile h in contadores)
            {
                if (hC == h._hC)
                {
                    if (hC.GetComponent<PlayerCountdown>().avaliableTime.Value > 0f)
                    {
                        StartCoroutine(RespawnRoutine(h)); 
                    }
                    else
                    {
                        Debug.Log("Nos fuimos");
                        StopAllCoroutines();
                        StartCoroutine(KillPlayer(h));
                    }
                }

            }
        }
    }
    public IEnumerator KillPlayer(HealthProfile h)
    {
        yield return new WaitForSeconds(.1f);
        h._pC.GetController().enabled = false;
        yield return new WaitForSeconds(1);
        h._pC.gameObject.transform.position = new Vector3(500f, 500f, 500f);
        h._pC.gameObject.transform.position = new Vector3(500f, 500f, 500f);
        h._pC.gameObject.transform.position = new Vector3(500f, 500f, 500f);
        yield return new WaitForSeconds(1);
        h._hC.HP.Value = startingHP;
        h._pC.GetComponent<PlayerInput>().enabled = true;
    }

    public IEnumerator RespawnRoutine(HealthProfile h)
    {
        h._pC.GetController().enabled = false;
        yield return new WaitForSeconds(1);
        h._pC.gameObject.transform.position = new Vector3(-37f, 3.9f, 41f);
        h._pC.gameObject.transform.position = new Vector3(-37f, 3.9f, 41f);
        h._pC.gameObject.transform.position = new Vector3(-37f, 3.9f, 41f);
        yield return new WaitForSeconds(1);
        h._hC.HP.Value = startingHP;
        h._pC.GetController().enabled = true;
    }

    public class HealthProfile
    {
        public HealthController _hC;
        public PlayerController _pC;

        public HealthProfile(HealthController hC, PlayerController pC)
        {
            _hC = hC;
            _pC = pC;

        }
    }
}
