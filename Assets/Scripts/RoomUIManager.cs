using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HelloWorldManager : MonoBehaviour
{
    const int maxConnections = 50;
    string joinCode = "Enter room code...";

    public Button hostButton;
    public Button joinButton;
    public TMP_InputField field;

    public GameObject lobbyInterf;
    public TextMeshProUGUI code;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            lobbyInterf.SetActive(false);
            code.text = "Codigo: " + joinCode;

        }

        GUILayout.EndArea();
    }

    private void Start()
    {

        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        field.onValueChanged.AddListener(ChangeCode);

    }

    void StartButtons()
    {
        //if (GUILayout.Button("Host")) StartHost();
        //if (GUILayout.Button("Client")) StartClient();
        //joinCode = GUILayout.TextField(joinCode);

        //if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    private void ChangeCode(string arg0)
    {
        joinCode = field.text;
    }
    async void StartHost()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "wss"));
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        NetworkManager.Singleton.StartHost();
    }

    async void StartClient()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "wss"));

        NetworkManager.Singleton.StartClient();
    }

    void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

       // GUILayout.Label("Transport: " +
      //      NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
      //  GUILayout.Label("Mode: " + mode);

       // GUILayout.Label("Room: " + joinCode);
    }
}

