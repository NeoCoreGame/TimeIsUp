using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EscurridizoBehaviourTres : BehaviourRunner, IEnemyBehaviour
{

    BSRuntimeDebugger _debugger;

    [HideInInspector] public GameObject _player;
    private PlayerController _pC;

    [SerializeField] Collider _visionCollider;
    //[SerializeField] Collider _attackCollider;

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

    int random = Random.Range(0, 5);
    [HideInInspector] public bool atacarPlayer;
    [HideInInspector] public bool dashear;

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
        //rangoAtaque = _attackCollider.transform.localScale.x;

        valorAtacado = _enemy.Hp.Value;


        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
	{
		var Escurridizo = new FSM();
		var EscurridizoPatrullar = new BehaviourTree();
		
		var patrullar_action = new SubsystemAction(EscurridizoPatrullar, true, ExecutionInterruptOptions.None);
		var patrullar = Escurridizo.CreateState("patrullar", patrullar_action);
		
		var huir_action = new FunctionalAction();
		huir_action.onStarted = StartHuir;
		huir_action.onUpdated = UpdateHuir;
		var huir = Escurridizo.CreateState("huir", huir_action);
		
		var dash_action = new FunctionalAction();
		dash_action.onStarted = StartDash;
		dash_action.onUpdated = UpdateDash;
		var dash = Escurridizo.CreateState("dash", dash_action);
		
		var dashear_perception = new ConditionPerception();
		dashear_perception.onCheck = Dash;
		var dashear = Escurridizo.CreateTransition("dashear", huir, dash, dashear_perception);
		
		var dash_huir_perception = new ConditionPerception();
		dash_huir_perception.onCheck = DashFinished;
		var dash_huir = Escurridizo.CreateTransition("dash_huir", dash, huir, dash_huir_perception);
		
		var jugadorVisto_perception = new ConditionPerception();
		jugadorVisto_perception.onCheck = playerClose;
		var jugadorVisto = Escurridizo.CreateTransition("jugadorVisto", patrullar, huir, jugadorVisto_perception);
		
		var atacar_action = new FunctionalAction();
		atacar_action.onStarted = StartAtacar;
		atacar_action.onUpdated = UpdateAtacar;
		var atacar = Escurridizo.CreateState("atacar", atacar_action);
		
		var huir_Atacar_perception = new ConditionPerception();
		huir_Atacar_perception.onCheck = attack;
		var huir_Atacar = Escurridizo.CreateTransition("huir_Atacar", huir, atacar, huir_Atacar_perception);
		
		var atacar_Huir_perception = new ConditionPerception();
		atacar_Huir_perception.onCheck = AttackFinished;
		var atacar_Huir = Escurridizo.CreateTransition("atacar_Huir", atacar, huir, atacar_Huir_perception);
		
		var primerPunto_action = new FunctionalAction();
		primerPunto_action.onStarted = PrimerStart;
		primerPunto_action.onUpdated = PrimerUpdate;
		var primerPunto = EscurridizoPatrullar.CreateLeafNode("primerPunto", primerPunto_action);
		
		var segundoPunto_action = new FunctionalAction();
		segundoPunto_action.onStarted = SegundoStart;
		segundoPunto_action.onUpdated = SegundoUpdate;
		var segundoPunto = EscurridizoPatrullar.CreateLeafNode("segundoPunto", segundoPunto_action);
		
		var tercerPunto_action = new FunctionalAction();
		tercerPunto_action.onStarted = TercerStart;
		tercerPunto_action.onUpdated = TercerUpdate;
		var tercerPunto = EscurridizoPatrullar.CreateLeafNode("tercerPunto", tercerPunto_action);
		
		var cuartoPunto_action = new FunctionalAction();
		cuartoPunto_action.onStarted = CuartoStart;
		cuartoPunto_action.onUpdated = CuartoUpdate;
		var cuartoPunto = EscurridizoPatrullar.CreateLeafNode("cuartoPunto", cuartoPunto_action);
		
		var secuencia = EscurridizoPatrullar.CreateComposite<SequencerNode>("secuencia", false, primerPunto, segundoPunto, tercerPunto, cuartoPunto);
		secuencia.IsRandomized = false;
		
		var iteraciones = EscurridizoPatrullar.CreateDecorator<LoopNode>("iteraciones", secuencia);
		iteraciones.Iterations = -1;

        EscurridizoPatrullar.SetRootNode(iteraciones);

        _debugger.RegisterGraph(Escurridizo);
        _debugger.RegisterGraph(EscurridizoPatrullar);

        return Escurridizo;
	}

    private void StartHuir()
    {
        Vector3 dir = (transform.position - _player.transform.position).normalized;
        finalPosition = transform.position + dir;

        _meshAgent.SetDestination(finalPosition);
    }

    private Status UpdateHuir()
    {
        if(Time.deltaTime > 5)
        {
            random = Random.Range(0, 5);
        }


        Vector3 dir = (transform.position - _player.transform.position).normalized;
        finalPosition = transform.position + dir;

        if (HasArrived())
        {
            dir = (transform.position - _player.transform.position).normalized;

            finalPosition = transform.position + dir;

            _meshAgent.SetDestination(finalPosition);
        }

        return Status.Running;
    }

    private void StartDash()
    {
        _meshAgent.speed = 7f;
    }

    private Status UpdateDash()
    {
        //Nada
        Invoke("ReduceSpeed", 1f);
        dashear = false;
        return Status.Success;
    }

    public void ReduceSpeed()
    {
        _meshAgent.speed = 3.5f;
    }


    private Boolean Dash()
    {
        if(random != 0)
        {
            dashear = true;
        }

        return dashear;
    }

    private Boolean DashFinished()
    {
        return !dashear;
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
        if(random == 0)
        {
            atacarPlayer = true;
        }

        return atacarPlayer;
    }

    private Boolean AttackFinished()
    {
        return !atacarPlayer;
    }

    public bool HasArrived()
    {
        Debug.Log(Vector3.Distance(finalPosition, transform.position));
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
            Debug.Log("Deberia acabar");
            return Status.Success;
        }
        Debug.Log("Sigue");
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
}
