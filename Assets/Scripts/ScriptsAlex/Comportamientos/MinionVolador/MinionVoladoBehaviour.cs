using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.UnityToolkit.GUIDesigner.Framework;
using BehaviourAPI.BehaviourTrees;
using SimpleAction = BehaviourAPI.Core.Actions.SimpleAction;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System.Diagnostics;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MinionVoladoBehaviour : BehaviourRunner
{
	[SerializeField] private Transform outRange_perception_OtherTransform;

    BSRuntimeDebugger _debugger;

    [SerializeField] private Transform primerPunto;
    [SerializeField] private Transform segundoPunto;
    [SerializeField] private Transform tercerPunto;

    [SerializeField] GameObject _player;
    [SerializeField] Collider _visionCollider;

    private Enemy _enemy;

    private NavMeshAgent _meshAgent;

    private Vector3 finalPosition;
    private float _speed;
    private float arrivingOffset = 6f;
	private float attackRange = 10f;

	private bool ataqueTerminado;
    private bool atacado;
    private int valorAtacado;

    private float rangoVision;
    private bool jugadorVisto;

    private bool goStunned;

    protected override void Init()
    {
        _debugger = GetComponent<BSRuntimeDebugger>();

        outRange_perception_OtherTransform = _player.GetComponent<Transform>();
        _enemy = GetComponent<Enemy>();

        _meshAgent = GetComponent<NavMeshAgent>();
        _speed = 3f;
        _meshAgent.speed = _speed;

        rangoVision = _visionCollider.transform.localScale.x;

        valorAtacado = _enemy.Hp.Value;


        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
	{
		var Minion_Volador = new FSM();
		var Minion_Volador_Patrolling = new BehaviourTree();
		
		var Patrullar_action = new SubsystemAction(Minion_Volador_Patrolling);
		var Patrullar = Minion_Volador.CreateState("Patrullar", Patrullar_action);
		
		var Avanzando_action = new FunctionalAction();
		Avanzando_action.onStarted = AvanzandoStart;
		Avanzando_action.onUpdated = AvanzandoUpdate;
		var Avanzando = Minion_Volador.CreateState("Avanzando", Avanzando_action);
		
		var Atacar_action = new FunctionalAction();
		Atacar_action.onStarted = AtacarStart;
		Atacar_action.onUpdated = AtacarUpdate;
		var Atacar = Minion_Volador.CreateState("Atacar", Atacar_action);
		
		var Aturdido_action = new SimpleAction();
		Aturdido_action.action = stunned;
		var Aturdido = Minion_Volador.CreateState("Aturdido", Aturdido_action);
		
		var jugadorEncontrado_perception_sub1 = new ConditionPerception();
		jugadorEncontrado_perception_sub1.onCheck = playerSeen;
		var jugadorEncontrado_perception_sub2 = new ConditionPerception();
		jugadorEncontrado_perception_sub2.onCheck = beingAttacked;
		var jugadorEncontrado_perception = new OrPerception(jugadorEncontrado_perception_sub1, jugadorEncontrado_perception_sub2);
		var jugadorEncontrado = Minion_Volador.CreateTransition("jugadorEncontrado", Patrullar, Avanzando, jugadorEncontrado_perception);
		
		var jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		var jugadorPerdido = Minion_Volador.CreateTransition("jugadorPerdido", Avanzando, Patrullar, jugadorPerdido_perception);
		
		var outRange_perception = new DistancePerception();
		outRange_perception.OtherTransform = outRange_perception_OtherTransform;
		outRange_perception.MaxDistance = 10f;
		var outRange = Minion_Volador.CreateTransition("outRange", Avanzando, Atacar, outRange_perception);
		
		var onRange = Minion_Volador.CreateTransition("onRange", Avanzando, Atacar, statusFlags: StatusFlags.Success);
		
		var entrarAturdido_perception = new ConditionPerception();
		entrarAturdido_perception.onCheck = beingStunned;
		var entrarAturdido = Minion_Volador.CreateTransition("entrarAturdido", Patrullar, Aturdido, entrarAturdido_perception);
		
		var salirAturdido_perception = new UnityTimePerception();
		salirAturdido_perception.TotalTime = 2f;
		var salirAturdido_action = new SimpleAction();
		salirAturdido_action.action = notStunned;
		var salirAturdido = Minion_Volador.CreateTransition("salirAturdido", Aturdido, Atacar, salirAturdido_perception, salirAturdido_action, statusFlags: StatusFlags.Success);
		
		var Primer_Punto_action = new FunctionalAction();
		Primer_Punto_action.onStarted = StartPrimer;
		Primer_Punto_action.onUpdated = UpdatePrimer;
		var Primer_Punto = Minion_Volador_Patrolling.CreateLeafNode("Primer Punto", Primer_Punto_action);
		
		var Segundo_Punto_action = new FunctionalAction();
		Segundo_Punto_action.onStarted = StartSegundo;
		Segundo_Punto_action.onUpdated = UpdateSegundo;
		var Segundo_Punto = Minion_Volador_Patrolling.CreateLeafNode("Segundo Punto", Segundo_Punto_action);
		
		var Tercer_Punto_action = new FunctionalAction();
		Tercer_Punto_action.onStarted = StartTercer;
		Tercer_Punto_action.onUpdated = UpdateTercer;
		var Tercer_Punto = Minion_Volador_Patrolling.CreateLeafNode("Tercer Punto", Tercer_Punto_action);
		
		var seq = Minion_Volador_Patrolling.CreateComposite<SequencerNode>("seq", false, Primer_Punto, Segundo_Punto, Tercer_Punto);
		seq.IsRandomized = false;
		
		var iter = Minion_Volador_Patrolling.CreateDecorator<LoopNode>("iter", seq);
		iter.Iterations = -1;

        _debugger.RegisterGraph(Minion_Volador);
        return Minion_Volador;
	}

    public bool HasArrived()
    {
        if (Vector3.Distance(finalPosition, transform.position) < arrivingOffset)
        {
            return true;
        }

        return false;
    }

	public bool onRange()
	{
        if (Vector3.Distance(finalPosition, transform.position) <= attackRange)
        {
            return true;
        }

        return false;
    }

    private void AvanzandoStart()
	{
        finalPosition = _player.GetComponent<Transform>().position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status AvanzandoUpdate()
	{
		if (onRange())
		{
			return Status.Success;
		}

		return Status.Running;
	}
	
	private void AtacarStart()
	{
        _enemy.hitted = false;
        //Animacion atacar

        _player.GetComponent<PlayerController>().TakeDamage(_enemy.GetDmg());
		//poner a true cuando termine el ataque
		ataqueTerminado = true;
    }
	
	private Status AtacarUpdate()
	{
		if (ataqueTerminado)
		{
			ataqueTerminado = false;
			return Status.Success;
		}

		return Status.Running;
	}
	
	private void stunned()
	{
        _meshAgent.isStopped = true;
		// Parar animaciones
    }
    private void notStunned()
    {
        _meshAgent.isStopped = false;
        goStunned = false;
		// Volver las animaciones
    }

    private Boolean playerSeen()
	{
		jugadorVisto = false;

        if (_visionCollider.bounds.Contains(_player.GetComponent<Transform>().position))
        {
            Vector3 direction = (_player.GetComponent<Transform>().position - transform.position).normalized;
            Ray ray = new Ray(transform.position + transform.up, direction * 20);

            jugadorVisto = Physics.Raycast(ray, out RaycastHit hit, rangoVision) && hit.collider.gameObject.transform == _player;

            return jugadorVisto;
        }

        return jugadorVisto;
    }
	
	private Boolean beingAttacked()
	{
        if (_enemy.Hp.Value < valorAtacado)
        {
            atacado = true;
            valorAtacado = _enemy.Hp.Value;
        }

        return atacado;
    }
	
	private Boolean playerLost()
	{
        return !beingAttacked() && !playerSeen();
    }
	
	private Boolean beingStunned()
	{
		goStunned = false;
        if (_enemy.Hp.Value < _enemy.hpThreshold && _enemy.GetHit())
        {
            goStunned = true;
        }

        return goStunned;
    }
	
	private void StartPrimer()
	{
        finalPosition = primerPunto.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdatePrimer()
	{
        if (HasArrived())
        {
            return Status.Success;
        }

        return Status.Running;
    }
	
	private void StartSegundo()
	{
        finalPosition = segundoPunto.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdateSegundo()
	{
        if (HasArrived())
        {
            return Status.Success;
        }

        return Status.Running;
    }
	
	private void StartTercer()
	{
        finalPosition = tercerPunto.position;
        _meshAgent.SetDestination(finalPosition);
    }
	
	private Status UpdateTercer()
	{
        if (HasArrived())
        {
            return Status.Success;
        }

        return Status.Running;
    }
}
