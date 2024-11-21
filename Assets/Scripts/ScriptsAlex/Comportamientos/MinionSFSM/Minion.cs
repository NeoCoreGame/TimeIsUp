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
using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine.UIElements;

public class Minion : BehaviourRunner
{
    [SerializeField] private Transform Avanzando_action_target;

    [SerializeField] PlayerController _playerController;
    private Enemy _enemy;

    private NavMeshAgent _meshAgent;
    //private Transform[] destinies;
    //private Vector3 _position;


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
        _meshAgent = GetComponent<NavMeshAgent>();

        valorAtacado = _enemy.Hp.Value;

        //destinies = FindObjectOfType<Destinies>().desinyGroup;


        base.Init();
    }
    protected override BehaviourGraph CreateGraph()
	{
		StackFSM Minion = new StackFSM();
		
		PatrolAction Deambular_action = new PatrolAction();
		Deambular_action.maxDistance = 10f;
		State Deambular = Minion.CreateState("Deambular", Deambular_action);
		
		ChaseAction Avanzando_action = new ChaseAction();
		Avanzando_action.speed = _meshAgent.speed;
		Avanzando_action.target = _playerController.transform;
		Avanzando_action.maxDistance = 0.5f;
		Avanzando_action.maxTime = 600f;
		State Avanzando = Minion.CreateState("Avanzando", Avanzando_action);
		
		ConditionPerception jugadorEncontrado_perception_sub1 = new ConditionPerception();
		jugadorEncontrado_perception_sub1.onCheck = playerClose;
		ConditionPerception jugadorEncontrado_perception_sub2 = new ConditionPerception();
		jugadorEncontrado_perception_sub2.onCheck = beingAttacked;
		OrPerception jugadorEncontrado_perception = new OrPerception(jugadorEncontrado_perception_sub1, jugadorEncontrado_perception_sub2);
		StateTransition jugadorEncontrado = Minion.CreateTransition("jugadorEncontrado", Deambular, Avanzando, jugadorEncontrado_perception);
		
		ConditionPerception jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		StateTransition jugadorPerdido = Minion.CreateTransition("jugadorPerdido", Avanzando, Deambular, jugadorPerdido_perception);
		
		SimpleAction Atacando_action = new SimpleAction();
		Atacando_action.action = attacking;
		State Atacando = Minion.CreateState("Atacando", Atacando_action);
		
		ConditionPerception entrarAtacando_perception = new ConditionPerception();
		entrarAtacando_perception.onCheck = onObjective;
		StateTransition entrarAtacando = Minion.CreateTransition("entrarAtacando", Avanzando, Atacando, entrarAtacando_perception);
		
		ConditionPerception ataqueTerminado_perception = new ConditionPerception();
		ataqueTerminado_perception.onCheck = attackFinished;
		StateTransition ataqueTerminado = Minion.CreateTransition("ataqueTerminado", Atacando, Avanzando, ataqueTerminado_perception);
		
		SimpleAction Aturdido_action = new SimpleAction();
		Aturdido_action.action = stunned;
		State Aturdido = Minion.CreateState("Aturdido", Aturdido_action);
		
		ConditionPerception entrarAturdido_perception = new ConditionPerception();
		entrarAturdido_perception.onCheck = goingStunned;
		PushTransition entrarAturdido = Minion.CreatePushTransition("entrarAturdido", Deambular, Aturdido, entrarAturdido_perception);
		
		ConditionPerception salirAturdido_perception = new ConditionPerception();
		salirAturdido_perception.onCheck = exitStunned;
		PopTransition salirAturdido = Minion.CreatePopTransition("salirAturdido", Aturdido, salirAturdido_perception);
		
		return Minion;
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

        return atacado;
    }

    private Boolean playerLost()
    {
        return !beingAttacked() && !playerClose();
    }

    private void attacking()
    {
        // Lanzar animación ataque


        _enemy.hitted = false;
        _playerController.TakeDamage(_enemy.GetDmg());

        ataqueFinalizado = true;

    }

    private Boolean onObjective()
    {
        enObjetivo = Physics.CheckSphere(transform.position, rangoAtaque, playerMask);

        return enObjetivo;
    }

    private Boolean attackFinished()
    {
        return ataqueFinalizado;
    }

    private void stunned()
    {
        _meshAgent.isStopped = true;
        // Para animaciones también

        stun();
        _meshAgent.isStopped = false;
        goStunned = false;
    }

    private Boolean goingStunned()
    {
        if (_enemy.Hp.Value < _enemy.hpThreshold && _enemy.GetHit())
        {
            goStunned = true;
        }

        return goStunned;
    }

    private Boolean exitStunned()
    {
        return goStunned;
    }

    IEnumerator stun()
    {
        yield return new WaitForSeconds(2);
    }
}
