using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ServerMessageManager : NetworkBehaviour
{

    [SerializeField]
    public NetworkVariable<FixedString4096Bytes> mensajesServidor = new NetworkVariable<FixedString4096Bytes>();

    //Texto
    public TextMeshProUGUI _mensajesDelServer;

    // Start is called before the first frame update
    void Start()
    {

        mensajesServidor.OnValueChanged += OnConsoleMessage;
        _mensajesDelServer = GetComponent<TextMeshProUGUI>();
    }

    private void OnConsoleMessage(FixedString4096Bytes previousValue, FixedString4096Bytes newValue)
    {
        //mensajesServidor.Value = newValue;
        _mensajesDelServer.text = mensajesServidor.Value.ToString();
    }


    public void ServerMessage(string message)
    {
        List<string> aux = new List<string>();
        if (mensajesServidor.Value != "")
        {
            aux = mensajesServidor.Value.ToString().Split('\n').ToList<string>();
            aux.Remove("");
        }
        aux.Add(message);

        if (aux.Count > 4)
        {
            aux.RemoveAt(0);
        }

        mensajesServidor.Value = "";
        foreach (string s in aux) { mensajesServidor.Value += s + '\n'; }



    }
}
