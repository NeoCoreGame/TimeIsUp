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

public class EscurridizoBehaviourDos : BehaviourRunner, IEnemyBehaviour
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
    private float arrivingOffset = 3f;

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


        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
	{
		FSM Escurridizo = new FSM();
		BehaviourTree EscurridizoPatrullar = new BehaviourTree();
		
		SubsystemAction patrullar_action = new SubsystemAction(EscurridizoPatrullar, true, ExecutionInterruptOptions.None);
		State patrullar = Escurridizo.CreateState(patrullar_action);
		
		FunctionalAction huir_action = new FunctionalAction();
		huir_action.onStarted = StartHuir;
		huir_action.onUpdated = UpdateHuir;
		State huir = Escurridizo.CreateState(huir_action);
		
		FunctionalAction dash_action = new FunctionalAction();
		dash_action.onStarted = StartDash;
		dash_action.onUpdated = UpdateDash;
		State dash = Escurridizo.CreateState(dash_action);
		
		ConditionPerception dashear_perception = new ConditionPerception();
		dashear_perception.onCheck = Dash;
		StateTransition dashear = Escurridizo.CreateTransition(huir, dash, dashear_perception);
		
		ConditionPerception dash_huir_perception = new ConditionPerception();
		dash_huir_perception.onCheck = DashFinished;
		StateTransition dash_huir = Escurridizo.CreateTransition(dash, huir, dash_huir_perception);
		
		ConditionPerception jugadorVisto_perception = new ConditionPerception();
		jugadorVisto_perception.onCheck = playerClose;
		StateTransition jugadorVisto = Escurridizo.CreateTransition(patrullar, huir, jugadorVisto_perception);
		
		FunctionalAction atacar_action = new FunctionalAction();
		atacar_action.onStarted = StartAtacar;
		atacar_action.onUpdated = UpdateAtacar;
		State atacar = Escurridizo.CreateState(atacar_action);
		
		ConditionPerception huir_Atacar_perception = new ConditionPerception();
		huir_Atacar_perception.onCheck = attack;
		StateTransition huir_Atacar = Escurridizo.CreateTransition(huir, atacar, huir_Atacar_perception);
		
		ConditionPerception atacar_Huir_perception = new ConditionPerception();
		atacar_Huir_perception.onCheck = AttackFinished;
		StateTransition atacar_Huir = Escurridizo.CreateTransition(atacar, huir, atacar_Huir_perception);
		
		FunctionalAction primerPunto_action = new FunctionalAction();
		primerPunto_action.onStarted = PrimerStart;
		primerPunto_action.onUpdated = PrimerUpdate;
		LeafNode primerPunto = EscurridizoPatrullar.CreateLeafNode(primerPunto_action);
		
		FunctionalAction segundoPunto_action = new FunctionalAction();
		segundoPunto_action.onStarted = SegundoStart;
		segundoPunto_action.onUpdated = SegundoUpdate;
		LeafNode segundoPunto = EscurridizoPatrullar.CreateLeafNode(segundoPunto_action);
		
		FunctionalAction tercerPunto_action = new FunctionalAction();
		tercerPunto_action.onStarted = TercerStart;
		tercerPunto_action.onUpdated = TercerUpdate;
		LeafNode tercerPunto = EscurridizoPatrullar.CreateLeafNode(tercerPunto_action);
		
		FunctionalAction cuartoPunto_action = new FunctionalAction();
		cuartoPunto_action.onStarted = CuartoStart;
		cuartoPunto_action.onUpdated = CuartoUpdate;
		LeafNode cuartoPunto = EscurridizoPatrullar.CreateLeafNode(cuartoPunto_action);
		
		SequencerNode secuencia = EscurridizoPatrullar.CreateComposite<SequencerNode>(false, primerPunto, segundoPunto, tercerPunto, cuartoPunto);
		secuencia.IsRandomized = false;
		
		LoopNode iteraciones = EscurridizoPatrullar.CreateDecorator<LoopNode>(secuencia);
		iteraciones.Iterations = -1;
		
		return Escurridizo;
	}
	
	private void StartHuir()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateHuir()
	{
		throw new System.NotImplementedException();
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


    private Boolean Dash()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean DashFinished()
	{
		throw new System.NotImplementedException();
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

    private Boolean AttackFinished()
	{
		throw new System.NotImplementedException();
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
