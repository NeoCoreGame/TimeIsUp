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
using UnityEngine.AI;
using Mono.Cecil.Cil;

public class MinionVoladorBehaviour : BehaviourRunner, IEnemyBehaviour
{
    [SerializeField] private Transform outRange_perception_OtherTransform;
    BSRuntimeDebugger _debugger;

    [HideInInspector] public GameObject _player;
    private PlayerController _pC;
    [SerializeField] Collider _visionCollider;
    [SerializeField] Collider _attackCollider;

    private Enemy _enemy;

    public Transform position;
    private NavMeshAgent _meshAgent;
    private Transform[] destinies;
    private Vector3 chosenDestiny;
    private Vector3 chosenDestinyTwo;

    public Transform groundCheck;
    [HideInInspector] public Vector3 finalPosition;
    private float _speed;
    private float arrivingOffset = 11f;

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

    private bool atacando;


    private bool goStunned;
    private Animator _animator;

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

        int r = UnityEngine.Random.Range(0, destinies.Length);
        chosenDestiny = destinies[r].position - new Vector3(-5f, 0f, 0f);
        chosenDestinyTwo = destinies[r].position - new Vector3(5f, 0f, 0f);

        _animator = GetComponent<Animator>();


        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
    {
        var MinionVolador = new FSM();
        var MinionVoladorPatrullar = new BehaviourTree();

        var Patrullar_action = new SubsystemAction(MinionVoladorPatrullar, true, ExecutionInterruptOptions.None);
        var Patrullar = MinionVolador.CreateState("Patrullar", Patrullar_action);

        var Avanzando_action = new FunctionalAction();
        Avanzando_action.onStarted = AvanzandoStart;
        Avanzando_action.onUpdated = AvanzandoUpdate;
        var Avanzando = MinionVolador.CreateState("Avanzando", Avanzando_action);

        var jugadorEncontrado_perception_sub1 = new ConditionPerception();
        jugadorEncontrado_perception_sub1.onCheck = playerSeen;
        var jugadorEncontrado_perception_sub2 = new ConditionPerception();
        jugadorEncontrado_perception_sub2.onCheck = beingAttacked;
        var jugadorEncontrado_perception = new OrPerception(jugadorEncontrado_perception_sub1, jugadorEncontrado_perception_sub2);
        var jugadorEncontrado = MinionVolador.CreateTransition("jugadorEncontrado", Patrullar, Avanzando, jugadorEncontrado_perception);

        var Atacando_action = new FunctionalAction();
        Atacando_action.onStarted = AtacarStart;
        Atacando_action.onUpdated = AtacarUpdate;
        var Atacando = MinionVolador.CreateState("Atacando", Atacando_action);

        var Aturdido_action = new SimpleAction();
        Aturdido_action.action = stunned;
        var Aturdido = MinionVolador.CreateState("Aturdido", Aturdido_action);

        var jugadorPerdido_perception = new ConditionPerception();
        jugadorPerdido_perception.onCheck = playerLost;
        var jugadorPerdido = MinionVolador.CreateTransition("jugadorPerdido", Avanzando, Patrullar, jugadorPerdido_perception);

        var entrarAturdido_perception = new ConditionPerception();
        entrarAturdido_perception.onCheck = beingStunned;
        var entrarAturdido = MinionVolador.CreateTransition("entrarAturdido", Avanzando, Aturdido, entrarAturdido_perception);

        var salirAturdido_perception = new UnityTimePerception();
        salirAturdido_perception.TotalTime = 2f;
        var salirAturdido = MinionVolador.CreateTransition("salirAturdido", Aturdido, Avanzando, salirAturdido_perception);

        var onRange_perception = new ConditionPerception();
        onRange_perception.onCheck = atacar;
        var onRange = MinionVolador.CreateTransition("onRange", Avanzando, Atacando, statusFlags: StatusFlags.Success);
        

        var attackFinished = new UnityTimePerception();
        attackFinished.TotalTime = 3f;
        var ataqueTerminado = MinionVolador.CreateTransition("ataqueTerminado", Atacando, Avanzando, attackFinished, statusFlags: StatusFlags.Success);

        //      var outRange_perception = new DistancePerception();
        //outRange_perception.OtherTransform = outRange_perception_OtherTransform;
        //outRange_perception.MaxDistance = 3f;
        //var outRange = MinionVolador.CreateTransition("outRange", Atacando, Avanzando, outRange_perception, statusFlags: StatusFlags.Finished);

        var PrimerPunto_action = new FunctionalAction();
        PrimerPunto_action.onStarted = StartPrimer;
        PrimerPunto_action.onUpdated = UpdatePrimer;
        var PrimerPunto = MinionVoladorPatrullar.CreateLeafNode("PrimerPunto", PrimerPunto_action);

        var SegundoPunto_action = new FunctionalAction();
        SegundoPunto_action.onStarted = StartSegundo;
        SegundoPunto_action.onUpdated = UpdateSegundo;
        var SegundoPunto = MinionVoladorPatrullar.CreateLeafNode("SegundoPunto", SegundoPunto_action);


        var Secuencia = MinionVoladorPatrullar.CreateComposite<SequencerNode>("Secuencia", false, PrimerPunto, SegundoPunto);
        Secuencia.IsRandomized = false;

        var Iteraciones = MinionVoladorPatrullar.CreateDecorator<LoopNode>("Iteraciones", Secuencia);
        Iteraciones.Iterations = -1;


        MinionVoladorPatrullar.SetRootNode(Iteraciones);

        _debugger.RegisterGraph(MinionVolador);


        return MinionVolador;
    }

    private void AvanzandoStart()
    {
        _meshAgent.isStopped = false;
        finalPosition = _player.transform.position;
        _meshAgent.SetDestination(finalPosition);
    }

    private Status AvanzandoUpdate()
    {
        finalPosition = _player.transform.position;
        _meshAgent.SetDestination(finalPosition);


        Debug.Log(Vector3.Distance(finalPosition, transform.position));


        if (HasArrived())
        {
            atacando = true;
            return Status.Success;
        }
        return Status.Running;
    }

    private Boolean playerSeen()
    {
        return jugadorVisto;
    }

    private Boolean beingAttacked()
    {
        return false;
    }

    private void AtacarStart()
    {
        _meshAgent.isStopped = true;
        _animator.SetTrigger("Attack");
    }

    public void HitPlayer(int dmg)
    {
        _pC.TakeDamage(dmg);
    }

    private Status AtacarUpdate()
    {
        atacando = false;
        return Status.Success;
    }

    private void stunned()
    {
        //nada
    }

    private Boolean playerLost()
    {
        return !jugadorVisto;
    }
    private Boolean atacar()
    {
        return atacando;
    }

    private Boolean beingStunned()
    {
        return goStunned;
    }

    private void StartPrimer()
    {


        finalPosition = chosenDestiny;
        _meshAgent.SetDestination(finalPosition);

    }
    public bool HasArrived()
    {

        if (Vector3.Distance(finalPosition, transform.position) < arrivingOffset)
        {
            return true;
        }


        return false;
    }

    private Status UpdatePrimer()
    {
        _meshAgent.SetDestination(finalPosition);


        if (HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    private void StartSegundo()
    {
        finalPosition = chosenDestinyTwo;
        _meshAgent.SetDestination(finalPosition);
    }

    private Status UpdateSegundo()
    {
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
        _pC = player.GetComponent<PlayerController>();
        outRange_perception_OtherTransform = player.transform;
    }

    public void CleanPlayer()
    {

        jugadorVisto = false;
    }
}
