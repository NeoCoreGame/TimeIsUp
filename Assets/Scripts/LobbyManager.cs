using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour //Clase que controla lo relacionado con el lobby
{
    //Variables 
    public static LobbyManager Instance { get; private set; } //Singleton de esta clase

    public TextMeshProUGUI jugadores; //Texto con el número de jugadores
    public TextMeshProUGUI votacionesHechas; //Texto con el número de votaciones por el momento

    public List<GameObject> playerObjects; //Lista de coches/jugadores en escena
    public List<PlayerInfo> playerInfos;

    [SerializeField]
    NetworkVariable<FixedString4096Bytes> players = new NetworkVariable<FixedString4096Bytes>(); //Network variable de nombres de todos los jugadores

    NetworkVariable<FixedString4096Bytes> votos = new NetworkVariable<FixedString4096Bytes>(); //Network variable de votos totales

    NetworkVariable<int> chosenCircuit = new NetworkVariable<int>(); //Network variable de circuito elegido

    NetworkVariable<bool> startGame = new NetworkVariable<bool>(); //Network variable de booleano que comienza la carrera
    NetworkVariable<bool> movePlayers = new NetworkVariable<bool>(); //Network variable que bloquea/desbloquea a los jugadores

    public int VotosFabrica; //Votos totales
    public int VotosBarco; //Votos totales
    public int VotosTotales;

    public bool finishedVote; //Indicador de que la votación ha terminado

    public GameObject[] circuits; //Array de los circuitos

    public GameObject[] buttons; //Botones de votación

    private enemySpawner spawner;
    private Destinies destinies;

    private InitialCountdown startCountdown;

    private RectTransform lobbyRect;

    public Transform[] factorySpawn;
    public Transform[] boatSpawn;

    private Transform[] electedSpawn;

    public Image mapF;
    public Image mapB;
    public Image mapLayout;
    private int selectedMapIdx;


    void Awake() //Configuración del singleton
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        //Suscribo funciones de cambio de valor

        players.OnValueChanged += OnTextChanged;
        votos.OnValueChanged += OnVote;

        spawner = FindObjectOfType<enemySpawner>();
        destinies = FindObjectOfType<Destinies>();

        startCountdown = FindObjectOfType<InitialCountdown>();



        lobbyRect = GetComponent<RectTransform>();

        


    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Invoke("PlayerVoteServerRpc",1.5f);
    }

    private void Update()
    {

        if (IsServer) //Si es el servidor
        {
            //Actualiza la lista de nombres de jugadores y los almacena en la networkVariable

            jugadores.text = "";
            jugadores.text = "Jugadores Conectados:";

            foreach (PlayerInfo p in playerInfos) { jugadores.text += "\n" + p.playerName; }
            players.Value = jugadores.text;



            //Cuando los votos se acaben, selecciono el circuito más votado

            if (VotosTotales == playerObjects.Count && playerObjects.Count != 0 && !finishedVote)
            {
                finishedVote = true;

                if (VotosFabrica > VotosBarco) { electedSpawn = factorySpawn; }
                else if (VotosFabrica < VotosBarco) { electedSpawn = boatSpawn; }
                else
                {
                    int chance = UnityEngine.Random.Range(0, 10);
                    if (chance < 6) { electedSpawn = factorySpawn; }
                    else
                    {
                        electedSpawn = boatSpawn;

                    }
                }

                votos.Value = "Votos acabados";
                votacionesHechas.text = "Votaciones: " + votos.Value.ToString();

                startGame.Value = true;

                Invoke("TeleportPlayers", 1f); //Coloco a los jugadores
                Invoke("TeleportPlayers", 3f); //Coloco a los jugadores

                Invoke("CountOneCountdown", 1f); //Los dejo caer
                Invoke("CountOneCountdown", 2f); //Los dejo caer
                Invoke("CountOneCountdown", 3f); //Los dejo caer
                Invoke("CountOneCountdown", 4f); //Los dejo caer
            }
            else if (!finishedVote) //Si no se ha votado, muestro los votos por el momento en el host
            {
                votos.Value = "" + VotosFabrica + " / " + playerObjects.Count;
                votacionesHechas.text = "Votaciones: " + votos.Value.ToString();
            }
        }
        else //Si no se ha votado, muestro los votos por el momento en los clientes
        {
            jugadores.text = players.Value.ToString();
            votacionesHechas.text = "Votaciones: " + votos.Value.ToString();
        }


        if (startCountdown.contador.Value == 3 && lobbyRect.localScale != Vector3.zero) //Si la carrera comienza, invoco funciones para prepararla
        {

            Cursor.lockState = CursorLockMode.Locked;

            lobbyRect.localScale = Vector3.zero;

        }

    }

    public void CountOneCountdown()
    {
        startCountdown.SubstractOne();

        if (startCountdown.contador.Value <= 0)
        {
            FreePlayers();
            StartRace();
            spawner.EnableSpawning(true);
            Invoke("RestartInitialCountdown", 2f);
        }
    }

    public void RestartInitialCountdown()
    {

        startCountdown.contador.Value = 4;
    }
    public void AddConnectedPlayers(GameObject newPlayer) //Funcion que añade a un jugador a la lista de coches
    {
        playerObjects.Add(newPlayer);
        playerInfos.Add(newPlayer.GetComponent<PlayerInfo>());

    }
    public void RemovePlayer(GameObject newPlayer) //Funcion que elimina a un jugador de la lista de coches
    {
        playerObjects.Remove(newPlayer);
        playerInfos.Add(newPlayer.GetComponent<PlayerInfo>());

    }


    private void OnTextChanged(FixedString4096Bytes previousValue, FixedString4096Bytes newValue) //Si players cambia, se ejecuta
    {
        players.Value = newValue;
    }
    private void OnVote(FixedString4096Bytes previousValue, FixedString4096Bytes newValue) //Si los votos cambia, se ejecuta
    {
        votos.Value = newValue;
    }

    public void TeleportPlayers() //Funcion que coloca a los jugadores en las posiciones del circuito
    {
        if (IsServer)
        {
            List<int> aux = new List<int>() { 0, 1, 2, 3 };

            foreach (GameObject p in playerObjects)
            {
                if (aux.Count == 0)
                {
                    aux = new List<int>() { 0, 1, 2, 3 };
                }

                int rNumber = UnityEngine.Random.Range(0, aux.Count);
                aux.Remove(rNumber);
                p.GetComponent<PlayerController>().TeleportPlayer(electedSpawn[rNumber].position);

            }

        }
    }

    public void FreePlayers() //Funcion que deja caer a los jugadores en el circuito
    {
        if (IsServer)
        {
            foreach (GameObject p in playerObjects)
            {

                p.GetComponent<PlayerController>().canMove.Value = true;
            }
        }
    }


    public void StartRace() //Funcion que comienza el cronómetro de la carrera
    {

        if (IsServer)
        {

            movePlayers.Value = true;
        }

    }

    public bool readyToRace() //Funcion que devuelve si los jugadores pueden moverse
    {
        return movePlayers.Value;
    }

    public void ReturnToLobby() //Funcion para devolver a los jugadores al lobby, restableciendo los valores necesarios
    {
        if (IsServer)
        {
            StopPlayers();

            spawner.EnableSpawning(false);

            ResetLobby();

            startGame.Value = false;
            movePlayers.Value = false;
        }

        gameObject.SetActive(true);

    }

    public void ResetLobby() //Funcion que resetea los votos y el circuito elegido
    {

        if (IsServer)
        {
            ResetVotesServerRpc();
            chosenCircuit.Value = 0;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetVotesServerRpc() //Llamada Rpc que restablece los votos
    {
        VotosFabrica = 0;
        finishedVote = false;

    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerVoteServerRpc() //Llamada Rpc que vota por NASCAR
    {

        if (selectedMapIdx == 0)
        {
            VotosFabrica += 1;
        }
        else
        {
            VotosBarco += 1;
        }
        VotosTotales++;

    }

    public void HideButtons() //Funcion que esconde los botones para votar
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(false);
        }
    }

    public void ShowButtons() //Funcion que muestra los botones para votar
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(true);
        }
    }

    public void StopPlayers() //Funcion que congela a los rigidbodies de los coches
    {
        if (IsServer)
        {
            movePlayers.Value = false;

            foreach (GameObject p in playerObjects)
            {

                p.GetComponent<PlayerController>().canMove.Value = false;
            }

        }
    }

    public void SelectMap(int mapIdx)
    {
        if (mapIdx == 0)
        {
            mapLayout.rectTransform.position = mapF.transform.position;
        }
        else if (mapIdx == 1)
        {
            mapLayout.rectTransform.position = mapB.transform.position;
        }
        selectedMapIdx = mapIdx;
    }
}

