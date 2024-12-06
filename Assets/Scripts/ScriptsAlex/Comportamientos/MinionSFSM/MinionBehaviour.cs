using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;
using Random = UnityEngine.Random;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System.Collections;
using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MinionBehaviour : BehaviourRunner
{
    BSRuntimeDebugger _debugger;

    [HideInInspector] public GameObject _player;
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


    protected override void Init()
    {
        _debugger = GetComponent<BSRuntimeDebugger>();

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


        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
	{
		var Minion = new StackFSM();
		
		var Deambular_action = new FunctionalAction();
		Deambular_action.onStarted = StartDeambular;
		Deambular_action.onUpdated = UpdateDeambular;
		var Deambular = Minion.CreateState("Deambular", Deambular_action);
		
		var Avanzando_action = new FunctionalAction();
		Avanzando_action.onStarted = StartAvanzando;
		Avanzando_action.onUpdated = UpdateAvanzando;
		var Avanzando = Minion.CreateState("Avanzando", Avanzando_action);
		
		var jugadorEncontrado_perception_sub1 = new ConditionPerception();
		jugadorEncontrado_perception_sub1.onCheck = playerClose;
		var jugadorEncontrado_perception_sub2 = new ConditionPerception();
		jugadorEncontrado_perception_sub2.onCheck = beingAttacked;
		var jugadorEncontrado_perception = new OrPerception(jugadorEncontrado_perception_sub1, jugadorEncontrado_perception_sub2);
		var jugadorEncontrado = Minion.CreateTransition("Jugador Encontrado", Deambular, Avanzando, jugadorEncontrado_perception);
		
		var jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		var jugadorPerdido = Minion.CreateTransition("Jugador Perdido", Avanzando, Deambular, jugadorPerdido_perception);
		
		var Atacando_action = new FunctionalAction();
		Atacando_action.onStarted = StartAtacando;
		Atacando_action.onUpdated = UpdateAtacando;
		var Atacando = Minion.CreateState("Atacando", Atacando_action);
		
		var entrarAtacando_perception = new ConditionPerception();
		entrarAtacando_perception.onCheck = onObjective;
		var entrarAtacando = Minion.CreateTransition("entrarAtacando", Avanzando, Atacando, entrarAtacando_perception);
		
		var ataqueTerminado_perception = new ConditionPerception();
		ataqueTerminado_perception.onCheck = attackFinished;
		var ataqueTerminado = Minion.CreateTransition("Salir Atacando", Atacando, Avanzando, ataqueTerminado_perception);
		
		var Aturdido_action = new FunctionalAction();
		Aturdido_action.onStarted = StartAturdido;
		Aturdido_action.onUpdated = UpdateAturdido;
		var Aturdido = Minion.CreateState("Aturdido", Aturdido_action);
		
		var entrarAturdido_perception = new ConditionPerception();
		entrarAturdido_perception.onCheck = goingStunned;
		var entrarAturdido = Minion.CreatePushTransition("Entrar Aturdido", Deambular, Aturdido, entrarAturdido_perception);
		
		var salirAturdido_perception = new ConditionPerception();
		salirAturdido_perception.onCheck = exitStunned;
		var salirAturdido = Minion.CreatePopTransition("Salir Aturdido", Aturdido, salirAturdido_perception);

        _debugger.RegisterGraph(Minion);
        return Minion;
	}
	
	private void StartDeambular()
	{
        finalPosition = destinies[Random.Range(0, destinies.Length)].position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdateDeambular()
	{
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {

            finalPosition = destinies[Random.Range(0, destinies.Length)].position;

            _meshAgent.SetDestination(finalPosition);

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

    private void StartAvanzando()
	{
        finalPosition = _player.GetComponent<Transform>().position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdateAvanzando()
    {
        finalPosition = _player.GetComponent<Transform>().position;
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;

        

    }

    private void StartAtacando()
    {
        _enemy.hitted = false;
        // Lanzar animación ataque
    }

    private Status UpdateAtacando()
    {
        // Cuando acabe la animación de ataque
        animacion = true;
        if (animacion)
        {
            _player.GetComponent<PlayerController>().TakeDamage(_enemy.GetDmg());
            ataqueFinalizado = true;
            return Status.Success;
        }

        return Status.Running;
    }

	// Accion de estar aturdido
    private void StartAturdido()
    {
        _meshAgent.isStopped = true;
        // Para animaciones del enemigo también

    }

    private Status UpdateAturdido()
    {
        stun();
        _meshAgent.isStopped = false;
        goStunned = false;

        return Status.Success;
    }


    // Percepciones

    private bool playerClose()
	{
        return jugadorVisto;
    }

    

    private bool beingAttacked()
	{
        if (_enemy.Hp.Value < valorAtacado)
        {
            atacado = true;
            valorAtacado = _enemy.Hp.Value;
        }

        return atacado;
    }

    private bool playerLost()
    {
        return !playerClose();
    }

    private bool onObjective()
	{
        if (_attackCollider.bounds.Contains(_player.GetComponent<Transform>().position))
        {
            Vector3 direction = (_player.GetComponent<Transform>().position - transform.position).normalized;
            Ray ray = new Ray(transform.position + transform.up, direction * 20);

            enObjetivo = Physics.Raycast(ray, out RaycastHit hit, rangoAtaque) && hit.collider.gameObject.transform == _player;
        }
        

        return enObjetivo;
    }
	
	private bool attackFinished()
	{
        return ataqueFinalizado;
    }
	

	private bool goingStunned()
	{
        if (_enemy.Hp.Value < _enemy.hpThreshold && _enemy.GetHit())
        {
            goStunned = true;
        }

        return goStunned;
    }
	
	private bool exitStunned()
	{
        return !goStunned;
    }

    IEnumerator stun()
    {
        yield return new WaitForSeconds(2);
    }
}
