using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using UnityEngine.AI;
using System.Diagnostics;
using UnityEngine.UIElements;
using System.Collections;
using Unity.Netcode;

public interface IEnemyBehaviour
{
	void DetectPlayer(GameObject player);
	void CleanPlayer();

    void EnableBehaviour();
}


public class ExplosivoBehaviour : BehaviourRunner, IEnemyBehaviour
{

     public GameObject _player;
    private PlayerController _pC;
    [SerializeField] Collider _visionCollider;
    [SerializeField] Collider _attackCollider;

    private Enemy _enemy;

    public Transform position;
    private NavMeshAgent _meshAgent;
    private Transform[] destinies;

    public Transform groundCheck;
    [HideInInspector] public Vector3 finalPosition;
    private float _speed;
    private float arrivingOffset = 1f;

    private bool atacado;
    private int valorAtacado;

    private bool ataqueFinalizado;
    private bool animacion = false;

    public LayerMask groundMask;
    private float rangoVision;
    [HideInInspector] public bool jugadorVisto;

    public LayerMask playerMask;
    private float rangoAtaque;
    private bool enObjetivo;


    private bool goStunned;
    [HideInInspector] public bool explotarJugador;
    private Animator _animator;

    private bool muerte;

    protected override BehaviourGraph CreateGraph()
	{
		var Explosivo = new FSM();
		
        var Dormido_action = new FunctionalAction();
        Dormido_action.onStarted = StartDormido;
        Dormido_action.onUpdated = UpdateDormido;
        var Dormido = Explosivo.CreateState("Dormido", Dormido_action);

        var Persiguiendo_action = new FunctionalAction();
		Persiguiendo_action.onStarted = StartPersiguiendo;
		Persiguiendo_action.onUpdated = UpdatePersiguiendo;
		var Persiguiendo = Explosivo.CreateState("Persiguiendo", Persiguiendo_action);
		
		var JugadorDetectado_perception = new ConditionPerception();
		JugadorDetectado_perception.onCheck = playerClose;
		var JugadorDetectado = Explosivo.CreateTransition("JugadorDetectado", Dormido, Persiguiendo, JugadorDetectado_perception);
		
		var Explotar_action = new SimpleAction();
		Explotar_action.action = explode;
		var Explotar = Explosivo.CreateState("Explotar", Explotar_action);
		
		var JugadorCerca_perception = new ConditionPerception();
		JugadorCerca_perception.onCheck = onObjective;
		var JugadorCerca_action = new SimpleAction();
		JugadorCerca_action.action = prepareExplode;
		var JugadorCerca = Explosivo.CreateTransition("JugadorCerca", Persiguiendo, Explotar, JugadorCerca_perception, JugadorCerca_action);
		
		var entrarExplotar_perception = new UnityTimePerception();
		entrarExplotar_perception.TotalTime = 60f;
		var entrarExplotar = Explosivo.CreateTransition("entrarExplotar", Persiguiendo, Explotar, entrarExplotar_perception);
		
		var Moverse_action = new FunctionalAction();
		Moverse_action.onStarted = StartMoverse;
		Moverse_action.onUpdated = UpdateMoverse;
		var Moverse = Explosivo.CreateState("Moverse", Moverse_action);
		
		var Despertar_perception = new UnityTimePerception();
		Despertar_perception.TotalTime = 120f;
		var Despertar = Explosivo.CreateTransition("Despertar", Dormido, Moverse, Despertar_perception);
		
		var JugadorVisto_perception = new ConditionPerception();
		JugadorVisto_perception.onCheck = playerSeen;
		var JugadorVisto = Explosivo.CreateTransition("JugadorVisto", Moverse, Persiguiendo, JugadorVisto_perception);

        var IdlePerception1 = new ConditionPerception();
        IdlePerception1.onCheck = Muerte;
        var VolverDormido = Explosivo.CreateTransition("VolverDormido", Explotar, Dormido, IdlePerception1);

        var IdlePerception2 = new ConditionPerception();
        IdlePerception2.onCheck = Muerte;
        var mantenerseDormido = Explosivo.CreateTransition("mantenerseDormido", Dormido, Dormido, IdlePerception2);

        var IdlePerception3 = new ConditionPerception();
        IdlePerception3.onCheck = Muerte;
        var regresarDormido = Explosivo.CreateTransition("regresarDormido", Persiguiendo, Dormido, IdlePerception3);


        return Explosivo;
	}
    protected override void Init()
    {


        _enemy = GetComponent<Enemy>();

        position = GetComponent<Transform>();
        groundCheck = transform.GetChild(2);

        _meshAgent = GetComponent<NavMeshAgent>();
        _speed = 3f;
        _meshAgent.speed = _speed;

        destinies = FindObjectOfType<Destinies>().desinyGroup;

        rangoVision = _visionCollider.transform.localScale.x;
        rangoAtaque = _attackCollider.transform.localScale.x;

        valorAtacado = _enemy.Hp.Value;
        _animator = GetComponent<Animator>();



        base.Init();
    }



    private void StartDormido()
    {
        muerte = false;
    }

    private Status UpdateDormido()
    {
                

        return Status.Running;
    }
    private void StartPersiguiendo()
	{
        if (_player != null)
        {
            finalPosition = _player.transform.position;
        }
        _meshAgent.SetDestination(finalPosition);
        _meshAgent.speed = 3f;
    }
	
	private Status UpdatePersiguiendo()
    {
        if (_player != null)
        {
            finalPosition = _player.transform.position;
        }
        _meshAgent.SetDestination(finalPosition);

        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;

    }
    public bool HasArrived()
    {
        if (Vector3.Distance(finalPosition, transform.position) < arrivingOffset)
        {
            return true;
        }

        return false;
    }

    private Boolean playerClose()
	{
        return jugadorVisto;
    }
	
	private void explode()
    {
        _animator.SetTrigger("Attack");
	}
	
	private Boolean onObjective()
	{
        return explotarJugador;
	}
	
	private void prepareExplode()
	{
        //Lanzar animacion
        _meshAgent.speed = 0f;
    }
	
	private void StartMoverse()
	{
        if (_player != null)
        {
            finalPosition = _player.transform.position;
        }
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdateMoverse()
	{
        if (_player!= null)
        {
            finalPosition = _player.transform.position; 
        }
        _meshAgent.SetDestination(finalPosition);

        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }
	
	private Boolean playerSeen()
	{
        return jugadorVisto;
    }

    public void HitPlayer(int dmg)
    {
        if (_pC!= null)
        {
            _pC.TakeDamage(dmg); 
        }



        CleanPlayer();
        explotarJugador = false;

        _enemy.Hp.Value = 0;
    }

    public void DetectPlayer(GameObject player)
    {
        jugadorVisto = true;
        _player = player;
        _pC = player.GetComponent<PlayerController>();

    }

    public void CleanPlayer()
    {
        jugadorVisto = false;
        _pC = null;
        _player = null;
        _meshAgent.ResetPath();
    }

    private bool Muerte()
    {
        if (_enemy.Hp.Value <= 0)
        {
            muerte = true;
            transform.position = new Vector3(-500f, 0f, -500f);
            CleanPlayer();
            gameObject.SetActive(false);
        }
        return muerte;
    }

    public void EnableBehaviour()
    {
        if (NetworkManager.Singleton.IsServer) { enabled = true; }
    }
}
