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
using SimpleAction = BehaviourAPI.Core.Actions.SimpleAction;
using Random = UnityEngine.Random;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System.Diagnostics;
using System.Collections;

public class Minion : BehaviourRunner
{
	private PlayerController _playerController;
    private Enemy _enemy;

    private NavMeshAgent _meshAgent;
    private Transform[] destinies;
	private Vector3 _position;


    private PushPerception beingStunned;


    private bool atacado;
    private int valorAtacado;

    private bool ataqueFinalizado;


    public LayerMask groundMask;
    public float rangoVision;
	private bool jugadorVisto;

	public LayerMask playerMask;
	public float rangoAtaque;
    private bool enObjetivo;



    private bool goStunned;

    protected override void Init()
	{
		_enemy = GetComponent<Enemy>();
		_playerController = GetComponent<PlayerController>();
        _meshAgent = GetComponent<NavMeshAgent>();

		valorAtacado = _enemy.Hp.Value;


        destinies = FindObjectOfType<Destinies>().desinyGroup;
        

        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
	{
		FSM Minion = new FSM();
		
		SimpleAction Deambular_action = new SimpleAction();
		Deambular_action.action = randomMovement;
		State Deambular = Minion.CreateState(Deambular_action);
		
		SimpleAction Avanzando_action = new SimpleAction();
		Avanzando_action.action = persiguiendo;
		State Avanzando = Minion.CreateState(Avanzando_action);
		
		SimpleAction Aturdido_action = new SimpleAction();
		Aturdido_action.action = stunned;
		State Aturdido = Minion.CreateState(Aturdido_action);
		
		SimpleAction Atacando_action = new SimpleAction();
		Atacando_action.action = attacking;
		State Atacando = Minion.CreateState(Atacando_action);
		
		ConditionPerception perseguirJugador_perception_sub1 = new ConditionPerception();
		perseguirJugador_perception_sub1.onCheck = playerClose;
		ConditionPerception perseguirJugador_perception_sub2 = new ConditionPerception();
		perseguirJugador_perception_sub2.onCheck = beingAttacked;
		OrPerception perseguirJugador_perception = new OrPerception(perseguirJugador_perception_sub1, perseguirJugador_perception_sub2);
		StateTransition perseguirJugador = Minion.CreateTransition(Deambular, Avanzando, perseguirJugador_perception);
		
		ConditionPerception jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		StateTransition jugadorPerdido = Minion.CreateTransition(Avanzando, Deambular, jugadorPerdido_perception);
		
		ConditionPerception AtaqueTerminado_perception = new ConditionPerception();
		AtaqueTerminado_perception.onCheck = attackFinished;
		StateTransition AtaqueTerminado = Minion.CreateTransition(Atacando, Avanzando, AtaqueTerminado_perception);
		
		ConditionPerception cercaJugador_perception = new ConditionPerception();
		cercaJugador_perception.onCheck = onObjective;
		StateTransition cercaJugador = Minion.CreateTransition(Avanzando, Atacando, cercaJugador_perception);
		
		ConditionPerception estarAturdido_perception = new ConditionPerception();
		estarAturdido_perception.onCheck = goingStunned;
		StateTransition estarAturdido = Minion.CreateTransition(Avanzando, Aturdido, estarAturdido_perception);
		
		beingStunned = new PushPerception(estarAturdido);
		return Minion;
	}
	
	private void randomMovement()
	{
        _position = destinies[Random.Range(0, destinies.Length)].position;
        _meshAgent.SetDestination(_position);

        if (goingStunned())
        {
            beingStunned.Fire();
        }
        _enemy.hitted = false;
		randomMovement();
    }
	
	private void persiguiendo()
	{
		ataqueFinalizado = false;
        _meshAgent.SetDestination(_playerController.transform.position);

        if (goingStunned())
        {
            beingStunned.Fire();
        }
        _enemy.hitted = false;
    }
	
	private void stunned()
	{
		_meshAgent.isStopped = true;
		// Para animaciones también

		stun();
		_meshAgent.isStopped = false;
	}
	
	private void attacking()
	{
        // Lanzar animación ataque

		// Si en mitad de la animación se puede seguir stuneando
        if (goingStunned())
        {
            beingStunned.Fire();
        }
		_enemy.hitted = false;
        _playerController.TakeDamage(_enemy.GetDmg());

		ataqueFinalizado = true;

	}
	
	private Boolean playerClose()
	{
		jugadorVisto = Physics.CheckSphere(transform.position, rangoVision, playerMask);

		return jugadorVisto;
	}

    private Boolean beingAttacked()
    {

        if (_enemy.Hp.Value < valorAtacado)
        {
            atacado = true;
            valorAtacado = _enemy.Hp.Value;
        }

		if (goingStunned())
		{
            beingStunned.Fire();
        }

        return atacado;
    }

    private Boolean playerLost()
    {
        return !beingAttacked() && !playerClose();
    }

    private Boolean attackFinished()
    {
        return ataqueFinalizado;
    }

    private Boolean onObjective()
    {
		enObjetivo = Physics.CheckSphere(transform.position, rangoAtaque, playerMask); 

        return enObjetivo;
    }

    private Boolean goingStunned()
    {
        if (_enemy.Hp.Value < _enemy.hpThreshold && _enemy.GetHit())
        {
            goStunned = true;		
        }

        return goStunned;
    }

	IEnumerator stun()
	{
		yield return new WaitForSeconds(2);
	}
}
