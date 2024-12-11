using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;
using UnityEngine.AI;
using Unity.Netcode;

public class EscurridizoBehaviour : BehaviourRunner, IEnemyBehaviour
{
	[SerializeField] private Transform Huir_action_OtherTransform;


    [HideInInspector] public GameObject _player;
    private PlayerController _pC;
    [SerializeField] Collider _visionCollider;
    [SerializeField] Collider _attackCollider;

    private Enemy _enemy;

    public Transform position;
    private NavMeshAgent _meshAgent;
    public Transform[] destinies;

    public Transform groundCheck;
    [HideInInspector] public Vector3 finalPosition;
    private float _speed;
    private float arrivingOffset =3f;

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
	private Animator _animator;

    [HideInInspector] public bool atacarPlayer;
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
    protected override BehaviourGraph CreateGraph()
	{
		var Escurridizo = new FSM();
		var EscurridizoPatrullar = new BehaviourTree();
		
		var Patrullar_action = new SubsystemAction(EscurridizoPatrullar, true, ExecutionInterruptOptions.None);
		var Patrullar = Escurridizo.CreateState("Patrullar", Patrullar_action);
		
		var Huir_action = new FleeAction();
		Huir_action.OtherTransform = Huir_action_OtherTransform;
		Huir_action.speed = 0f;
		Huir_action.distance = 0f;
		Huir_action.maxTimeRunning = 0f;
		var Huir = Escurridizo.CreateState("Huir", Huir_action);
		
		var Dash_action = new FunctionalAction();
		Dash_action.onStarted = StartDash;
		Dash_action.onUpdated = UpdateDash;
		var Dash = Escurridizo.CreateState("Dash", Dash_action);
		
		var Dashear_perception = new DistancePerception();
		Dashear_perception.OtherTransform = Huir_action_OtherTransform;
		Dashear_perception.MaxDistance = 0f;
		var Dashear = Escurridizo.CreateTransition("Dashear", Huir, Dash, Dashear_perception);
		
		var Dash_Huir = Escurridizo.CreateTransition("Dash_Huir", Dash, Huir, statusFlags: StatusFlags.Success);
		
		var JugadorVisto_perception = new ConditionPerception();
		JugadorVisto_perception.onCheck = playerClose;
		var JugadorVisto = Escurridizo.CreateTransition("JugadorVisto", Patrullar, Huir, JugadorVisto_perception);
		
		var Atacar_action = new FunctionalAction();
		Atacar_action.onStarted = StartAtacar;
		Atacar_action.onUpdated = UpdateAtacar;
		var Atacar = Escurridizo.CreateState("Atacar", Atacar_action);
		
		var Atacar_1_perception = new ConditionPerception();
		Atacar_1_perception.onCheck = attack;
		var Atacar_1 = Escurridizo.CreateTransition("Atacar_1", Huir, Atacar, Atacar_1_perception);
		
		var Atacar_Huir = Escurridizo.CreateTransition("Atacar_Huir", Atacar, Huir, statusFlags: StatusFlags.Success);
		
		var PrimerPunto_action = new FunctionalAction();
		PrimerPunto_action.onStarted = PrimerStart;
		PrimerPunto_action.onUpdated = PrimerUpdate;
		var PrimerPunto = EscurridizoPatrullar.CreateLeafNode("PrimerPunto", PrimerPunto_action);
		
		var SegundoPunto_action = new FunctionalAction();
		SegundoPunto_action.onStarted = SegundoStart;
		SegundoPunto_action.onUpdated = SegundoUpdate;
		var SegundoPunto = EscurridizoPatrullar.CreateLeafNode("SegundoPunto", SegundoPunto_action);
		
		var TercerPunto_action = new FunctionalAction();
		TercerPunto_action.onStarted = TercerStart;
		TercerPunto_action.onUpdated = TercerUpdate;
		var TercerPunto = EscurridizoPatrullar.CreateLeafNode("TercerPunto", TercerPunto_action);
		
		var CuartoPunto_action = new FunctionalAction();
		CuartoPunto_action.onStarted = CuartoStart;
		CuartoPunto_action.onUpdated = CuartoUpdate;
		var CuartoPunto = EscurridizoPatrullar.CreateLeafNode("CuartoPunto", CuartoPunto_action);
		
		var Secuencia = EscurridizoPatrullar.CreateComposite<SequencerNode>("Secuencia", false, PrimerPunto, SegundoPunto, TercerPunto, CuartoPunto);
		Secuencia.IsRandomized = false;
		
		var iteraciones = EscurridizoPatrullar.CreateDecorator<LoopNode>("iteraciones", Secuencia);
		iteraciones.Iterations = -1;

        EscurridizoPatrullar.SetRootNode(iteraciones);
        return Escurridizo;
	}
	
	private void StartDash()
	{		
		_meshAgent.speed = 7f;

		Invoke("ReduceSpeed", 1f);
	}
	
	private Status UpdateDash()
	{
		//Nada
		return Status.Success;
	}

	public void ReduceSpeed()
	{

		_meshAgent.speed = 3.5f;
    }
	
	private Boolean playerClose()
	{
		return jugadorVisto;
	}
	
	private void StartAtacar()
	{
		//Lanzar animacion
		_pC.TakeDamage(10);
	}
	
	private Status UpdateAtacar()
	{
        //nada que añadir.
        atacarPlayer = false;
        return Status.Success;
    }
	
	private Boolean attack()
	{
		return atacarPlayer;
	}

    public bool HasArrived()
    {
        if (Vector3.Distance(finalPosition, transform.position) < arrivingOffset)
        {
            return true;
        }

        return false;
    }

    private void PrimerStart()
	{
		finalPosition = destinies[0].transform.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status PrimerUpdate()
    {
        finalPosition = destinies[0].transform.position;
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }

        return Status.Running;
    }
	
	private void SegundoStart()
	{
        finalPosition = destinies[1].transform.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status SegundoUpdate()
	{
        finalPosition = destinies[1].transform.position;
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }
	
	private void TercerStart()
	{
        finalPosition = destinies[2].transform.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status TercerUpdate()
	{
        finalPosition = destinies[2].transform.position;
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }
	
	private void CuartoStart()
	{
        finalPosition = destinies[3].transform.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status CuartoUpdate()
	{
        finalPosition = destinies[3].transform.position;
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    public void DetectPlayer(GameObject player)
    {
        jugadorVisto = true;
        _player = player;
    }

    public void CleanPlayer()
    {

        jugadorVisto = false;
    }

    public void EnableBehaviour()
    {
        if (NetworkManager.Singleton.IsServer) { enabled = true; }
    }
}
