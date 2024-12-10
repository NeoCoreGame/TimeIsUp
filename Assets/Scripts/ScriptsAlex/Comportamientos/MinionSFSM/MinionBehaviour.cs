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
using System.Linq;

public class MinionBehaviour : BehaviourRunner, IEnemyBehaviour
{
    BSRuntimeDebugger _debugger;

    [HideInInspector] public GameObject _player;
    private PlayerController _pC;
    [SerializeField] Collider _visionCollider;
    [SerializeField] Collider _attackCollider;

    private Enemy _enemy;

    public Transform position;
    private NavMeshAgent _meshAgent;
    private Transform[] destinies;
    private Transform chosenTransf;

    public Transform groundCheck;
    [HideInInspector] public Vector3 finalPosition;
    private float _speed;
    public float arrivingOffset = 4f;
    public float distance;

    private bool atacado;
    private int valorAtacado;

    private bool ataqueFinalizado;
    private bool animacion = false;

    public LayerMask groundMask;
    private float rangoVision;
    [HideInInspector] public bool jugadorVisto;

    public LayerMask playerMask;
    private float rangoAtaque;
    public bool enObjetivo;


    private bool goStunned;
    private bool deambularAcabado = false;
    private bool muerte = false;

    private Animator _anim;

    protected override void Init()
    {
        _debugger = GetComponent<BSRuntimeDebugger>();

        _enemy = GetComponentInChildren<Enemy>();

        position = GetComponent<Transform>();
        groundCheck = transform.GetChild(2);

        _meshAgent = GetComponent<NavMeshAgent>();
        _speed = 3f;
        _meshAgent.speed = _speed;

        destinies = FindObjectOfType<Destinies>().desinyGroup;

        rangoVision = _visionCollider.transform.localScale.x;
        rangoAtaque = _attackCollider.transform.localScale.x;

        valorAtacado = _enemy.Hp.Value;

        _anim =GetComponent<Animator>();
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

        var Deambular_Perception = new ConditionPerception();
        Deambular_Perception.onCheck = DeambularAcabado;

        var DeambularAcabado_Transition = Minion.CreateTransition("deambular_acabado_transition", Deambular, Deambular, Deambular_Perception);

        var IdlePerception = new ConditionPerception();
        IdlePerception.onCheck = Muerte;
        var VolverDeambular = Minion.CreateTransition("VolverDeambular", Avanzando, Deambular, IdlePerception);

        var IdlePerception2 = new ConditionPerception();
        IdlePerception2.onCheck = Muerte;
        var quedarseDeambular = Minion.CreateTransition("quedarseDeambular", Deambular, Deambular, IdlePerception2);


        _debugger.RegisterGraph(Minion);
        return Minion;
	}
    private bool DeambularAcabado()
    {
        return deambularAcabado;
    }
    private void StartDeambular()
    {
        deambularAcabado = false;
        muerte = false;
        atacado = false;
        ChooseDestiny();
       // _meshAgent.SetDestination(finalPosition);
    }
    private Status UpdateDeambular()
    {
        _meshAgent.SetDestination(finalPosition);

        distance = Vector3.Distance(finalPosition, transform.position);

        if (_enemy.Hp.Value <= 0)
        {
            muerte = true;
            transform.position = new Vector3(-500f, 0f, -500f);
            CleanPlayer();
            gameObject.SetActive(false);
        }

        if (HasArrived())
        {
            ChooseDestiny();

            _meshAgent.SetDestination(finalPosition);

            deambularAcabado = true;

            return Status.Success;
        }

        return Status.Running;
    }

    public void ChooseDestiny()
    {
        arrivingOffset = Random.Range(5, 7);
        List<Transform> transforms = destinies.ToList<Transform>();

        if (chosenTransf != null)
        {
            transforms.Remove(chosenTransf);
        }

        finalPosition = transforms[Random.Range(0, transforms.Count)].position;

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
        arrivingOffset = 4f;
        finalPosition = _player.transform.position;
       // _meshAgent.SetDestination(finalPosition);
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
        ataqueFinalizado = true;
        enObjetivo = false;
        _anim.SetTrigger("Attack");
    }

    private Status UpdateAtacando()
    {
        
            
            return Status.Success;

    }

	// Accion de estar aturdido
    private void StartAturdido()
    {
       // _meshAgent.isStopped = true;
        // Para animaciones del enemigo también

    }
    public void HitPlayer(int dmg)
    {
        _pC.TakeDamage(dmg);
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

    public void DetectPlayer(GameObject player)
    {
        jugadorVisto = true;
        _player = player;
        _pC = player.GetComponent<PlayerController>();
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

    public void CleanPlayer()
    {

        jugadorVisto = false;
    }
}
