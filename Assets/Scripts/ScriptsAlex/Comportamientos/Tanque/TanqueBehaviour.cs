using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.StateMachines;
using UnityEngine.AI;
using Unity.Netcode;

public class TanqueBehaviour : BehaviourRunner, IEnemyBehaviour
{
    [HideInInspector] public GameObject _player;
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

	[HideInInspector] public bool atacarPlayerFar;
    [HideInInspector] public bool atacarPlayerClose;

	private Animator _animator;

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
		var Tanque = new BehaviourTree();
		var Tanque_1 = new FSM();
		var TanqueAtaques = new BehaviourTree();
		var TanqueInvocado = new FSM();
		
		var NoInvocado_action = new SubsystemAction(Tanque_1, true, ExecutionInterruptOptions.None);
		var NoInvocado = Tanque.CreateLeafNode("NoInvocado", NoInvocado_action);

		
		var NoInvocado_1 = Tanque.CreateDecorator<ConditionNode>("NoInvocado_1", NoInvocado);
        var NoInvocadoPerception = new ConditionPerception();
        NoInvocadoPerception.onCheck = noInvocado;
        NoInvocado_1.SetPerception(NoInvocadoPerception);
		
		var Invocado_action = new SubsystemAction(TanqueInvocado, true, ExecutionInterruptOptions.None);
		var Invocado = Tanque.CreateLeafNode("Invocado", Invocado_action);
		
		var Invocado_1 = Tanque.CreateDecorator<ConditionNode>("Invocado_1", Invocado);
        var InvocadoPerception = new ConditionPerception();
        InvocadoPerception.onCheck = invocado;
        Invocado_1.SetPerception(InvocadoPerception);
	
		var Selector = Tanque.CreateComposite<SelectorNode>("Selector", false, NoInvocado_1, Invocado_1);
		Selector.IsRandomized = false;

    
        var Idle = Tanque_1.CreateState("Idle");
		
		var Perseguir_action = new FunctionalAction();
		Perseguir_action.onStarted = StartPerseguir;
		Perseguir_action.onUpdated = UpdatePerseguir;
		var Perseguir = Tanque_1.CreateState("Perseguir", Perseguir_action);
		
		var jugadorEncontrado_perception = new ConditionPerception();
		jugadorEncontrado_perception.onCheck = playerClose;
		var jugadorEncontrado = Tanque_1.CreateTransition("jugadorEncontrado", Idle, Perseguir, jugadorEncontrado_perception);
		
		var jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		var jugadorPerdido = Tanque_1.CreateTransition("jugadorPerdido", Perseguir, Idle, jugadorPerdido_perception);
		
		var Atacar_action = new SubsystemAction(TanqueAtaques);
		var Atacar = Tanque_1.CreateState("Atacar", Atacar_action);
		
		var entrarAtacar_perception = new ConditionPerception();
		entrarAtacar_perception.onCheck = onObjective;
		var entrarAtacar = Tanque_1.CreateTransition("entrarAtacar", Perseguir, Atacar, entrarAtacar_perception);
		
		var salirAtacar = Tanque_1.CreateTransition("salirAtacar", Atacar, Perseguir, statusFlags: StatusFlags.Success);
		
		var PrimerAtaque_1_action = new FunctionalAction();
		PrimerAtaque_1_action.onStarted = StartAtaqueBasico;
		PrimerAtaque_1_action.onUpdated = UpdateAtaqueBasico;
		var PrimerAtaque_1 = TanqueAtaques.CreateLeafNode("PrimerAtaque_1", PrimerAtaque_1_action);
		
		var PrimerAtaque = TanqueAtaques.CreateDecorator<ConditionNode>("PrimerAtaque", PrimerAtaque_1);
		
		var succederAtaques = TanqueAtaques.CreateDecorator<SuccederNode>("succederAtaques", PrimerAtaque);
		
		var AtaqueFuerte_action = new FunctionalAction();
		AtaqueFuerte_action.onStarted = StartAtaqueFuerte;
		AtaqueFuerte_action.onUpdated = UpdateAtaqueFuerte;
		var AtaqueFuerte = TanqueAtaques.CreateLeafNode("AtaqueFuerte", AtaqueFuerte_action);
		
		var AtaqueBasico_action = new FunctionalAction();
		AtaqueBasico_action.onStarted = StartAtaqueBasico;
		AtaqueBasico_action.onUpdated = UpdateAtaqueBasico;
		var AtaqueBasico = TanqueAtaques.CreateLeafNode("AtaqueBasico", AtaqueBasico_action);
		
		var secuenciaAtaques = TanqueAtaques.CreateComposite<SequencerNode>("secuenciaAtaques", false, AtaqueFuerte, AtaqueBasico);
		secuenciaAtaques.IsRandomized = false;
		
		var AtaquesBasicos_action = new FunctionalAction();
		AtaquesBasicos_action.onStarted = StartAtaquesBasicos;
		AtaquesBasicos_action.onUpdated = UpdateAtaquesBasicos;
		var AtaquesBasicos = TanqueAtaques.CreateLeafNode("AtaquesBasicos", AtaquesBasicos_action);
		
		var Seleccion3 = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion3", true, secuenciaAtaques, AtaquesBasicos);
		Seleccion3.IsRandomized = true;
		
		var mitadVida = TanqueAtaques.CreateDecorator<ConditionNode>("mitadVida", Seleccion3);
		
		var succederAtaques2 = TanqueAtaques.CreateDecorator<SuccederNode>("succederAtaques2", mitadVida);
		
		var AtaqueIndividual_action = new FunctionalAction();
		AtaqueIndividual_action.onStarted = StartAtaqueIndividual;
		AtaqueIndividual_action.onUpdated = UpdateAtaqueIndividual;
		var AtaqueIndividual = TanqueAtaques.CreateLeafNode("AtaqueIndividual", AtaqueIndividual_action);
		
		var Seleccion2 = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion2", false, succederAtaques2, AtaqueIndividual);
		Seleccion2.IsRandomized = false;
		
		var Seleccion = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion", false, succederAtaques, Seleccion2);
		Seleccion.IsRandomized = false;
		
		var Perseguir_1_action = new FunctionalAction();
		Perseguir_1_action.onStarted = StartPerseguir;
		Perseguir_1_action.onUpdated = UpdatePerseguir;
		var Perseguir_1 = TanqueInvocado.CreateState("Perseguir_1", Perseguir_1_action);
		
		var AtacarBasico_action = new FunctionalAction();
		AtacarBasico_action.onStarted = StartAtacar;
		AtacarBasico_action.onUpdated = UpdateAtacar;
		var AtacarBasico = TanqueInvocado.CreateState("AtacarBasico", AtacarBasico_action);
		
		var jugadorCerca_perception = new ConditionPerception();
		jugadorCerca_perception.onCheck = onObjective;
		var jugadorCerca = TanqueInvocado.CreateTransition("jugadorCerca", Perseguir_1, AtacarBasico, jugadorCerca_perception);
		
		var ataqueTerminado = TanqueInvocado.CreateTransition("ataqueTerminado", AtacarBasico, Perseguir_1, statusFlags: StatusFlags.Success);
		
		var AtacarFuerte_action = new FunctionalAction();
		AtacarFuerte_action.onStarted = StartAtacarFuerte;
		AtacarFuerte_action.onUpdated = UpdateAtacarFuerte;
		var AtacarFuerte = TanqueInvocado.CreateState("AtacarFuerte", AtacarFuerte_action);
		
		var ataqueFuerteTerminado = TanqueInvocado.CreateTransition("ataqueFuerteTerminado", AtacarFuerte, Perseguir_1, statusFlags: StatusFlags.Success);
		
		var jugadorMuyCerca_perception = new ConditionPerception();
		jugadorMuyCerca_perception.onCheck = onObjectiveClose;
		var jugadorMuyCerca = TanqueInvocado.CreateTransition("jugadorMuyCerca", Perseguir_1, AtacarFuerte, jugadorMuyCerca_perception);

        return Tanque;
	}

	

    private void StartPerseguir()
	{
        finalPosition = _player.transform.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdatePerseguir()
	{
        finalPosition = _player.transform.position;
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
	
	private Boolean playerLost()
	{
		return !jugadorVisto;
	}
	
	private Boolean onObjective()
	{
		return atacarPlayerFar;
	}
	
	private void StartAtaqueBasico()
	{
		//Lanzar animacion
		_pC.TakeDamage(20);
		_animator.SetTrigger("Attack");
	}
	
	private Status UpdateAtaqueBasico()
	{
		//nada que añadir.
		atacarPlayerFar = false;
		return Status.Success;
	}
	
	private void StartAtaqueFuerte()
	{
		//Lanzar animacion
        _pC.TakeDamage(50);
        _animator.SetTrigger("Attack_Hand");
    }
	
	private Status UpdateAtaqueFuerte()
	{
        //nada que añadir.
        atacarPlayerClose= false;
        return Status.Success;
    }
	
	private void StartAtaquesBasicos()
	{
		//LAnzar animacion 
		Invoke("AtacarPlayer", .5f);
        Invoke("AtacarPlayer", 1f);
        _animator.SetTrigger("Attack_DHand");
    }

	public void AtacarPlayer()
	{

        _pC.TakeDamage(20);
        _animator.SetTrigger("Attack_Hand");
    }
	
	private Status UpdateAtaquesBasicos()
	{
		// nada que añadir

		return Status.Success;
	}
	
	private void StartAtaqueIndividual()
    {
        _pC.TakeDamage(20);
    }
	
	private Status UpdateAtaqueIndividual()
    {
        // nada que añadir

        return Status.Success;
    }
	
	private void StartAtacar()
    {
        _pC.TakeDamage(20);
        _animator.SetTrigger("Attack");
    }
	
	private Status UpdateAtacar()
    {
        // nada que añadir

        return Status.Success;
    }
	
	private void StartAtacarFuerte()
    {
        _pC.TakeDamage(50);
        _animator.SetTrigger("Attack");
    }
	
	private Status UpdateAtacarFuerte()
    {
        // nada que añadir

        return Status.Success;
    }
	
	private Boolean onObjectiveClose()
	{

		return atacarPlayerClose;
	}

    private bool noInvocado()
    {
        return false;
    }

    private bool invocado()
    {
        return true;
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
    }

    public void EnableBehaviour()
    {
        if (NetworkManager.Singleton.IsServer) { enabled = true; }
    }
}
