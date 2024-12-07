using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.StateMachines;
using System.Diagnostics;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class TanqueBehaviourTres : BehaviourRunner, IEnemyBehaviour
{

    BSRuntimeDebugger _debugger;

    [HideInInspector] public GameObject _player;
    private PlayerController _pC;
    [SerializeField] Collider _visionCollider;
    [SerializeField] Collider _attackCollider;
    [SerializeField] Collider _attackColliderClose;

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

    public bool invocado = true;
    private bool primerAtaque = true;

    private int halfHp;
    private bool mitadVida = false;

    [HideInInspector] public bool atacarPlayerFar;
    [HideInInspector] public bool atacarPlayerClose;

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

        halfHp = _enemy.Hp.Value;
        _animator = GetComponent<Animator>();

        base.Init();
    }

    protected override BehaviourGraph CreateGraph()
    {
        var Tanque = new BehaviourTree();
        var Tanque_1 = new FSM();
        var TanqueAtaques = new BehaviourTree();
        var TanqueInvocado = new FSM();

        var noInvocado_action = new SubsystemAction(Tanque_1, true, ExecutionInterruptOptions.None);
        var noInvocado = Tanque.CreateLeafNode("noInvocado", noInvocado_action);

        var noInvocadoCondition = Tanque.CreateDecorator<ConditionNode>("noInvocadoCondition", noInvocado);
        var noInvocadoPerception = new ConditionPerception();
        noInvocadoPerception.onCheck = NoInvocado;
        noInvocadoCondition.SetPerception(noInvocadoPerception);

        var invocado_action = new SubsystemAction(TanqueInvocado, true, ExecutionInterruptOptions.None);
        var invocado = Tanque.CreateLeafNode("invocado", invocado_action);

        var invocadoCondition = Tanque.CreateDecorator<ConditionNode>("invocadoCondition", invocado);
        var invocadoPerception = new ConditionPerception();
        invocadoPerception.onCheck = Invocado;
        invocadoCondition.SetPerception(invocadoPerception);

        var selector = Tanque.CreateComposite<SelectorNode>("selector", false, noInvocadoCondition, invocadoCondition);
        selector.IsRandomized = false;

        var idle_action = new FunctionalAction();
        idle_action.onStarted = StartIdleAction;
        idle_action.onUpdated = UpdateIdleAction;
        var idle = Tanque_1.CreateState("idle", idle_action);

        var perseguir_action = new FunctionalAction();
        perseguir_action.onStarted = StartPerseguir;
        perseguir_action.onUpdated = UpdatePerseguir;
        var perseguir = Tanque_1.CreateState("perseguir", perseguir_action);

        var jugadorEncontrado_perception = new ConditionPerception();
        jugadorEncontrado_perception.onCheck = playerClose;
        var jugadorEncontrado = Tanque_1.CreateTransition("jugadorEncontrado", idle, perseguir, jugadorEncontrado_perception);

        var jugadorPerdido_perception = new ConditionPerception();
        jugadorPerdido_perception.onCheck = playerLost;
        var jugadorPerdido = Tanque_1.CreateTransition("jugadorPerdido", perseguir, idle, jugadorPerdido_perception);

        var atacarNoInvocado_action = new SubsystemAction(TanqueAtaques);
        var atacarNoInvocado = Tanque_1.CreateState("atacarNoInvocado", atacarNoInvocado_action);

        var entrarAtacar_perception = new ConditionPerception();
        entrarAtacar_perception.onCheck = onObjective;
        var entrarAtacar = Tanque_1.CreateTransition("entrarAtacar", perseguir, atacarNoInvocado, entrarAtacar_perception);

        var salirAtacar_perception = new ConditionPerception();
        salirAtacar_perception.onCheck = AttackFinished;
        var salirAtacar = Tanque_1.CreateTransition("salirAtacar", atacarNoInvocado, perseguir, salirAtacar_perception);

        var primerAtaque_action = new FunctionalAction();
        primerAtaque_action.onStarted = StartAtaqueBasico;
        primerAtaque_action.onUpdated = UpdateAtaqueBasico;
        var primerAtaque = TanqueAtaques.CreateLeafNode("primerAtaque", primerAtaque_action);

        var primerAtaqueCondition = TanqueAtaques.CreateDecorator<ConditionNode>("primerAtaqueCondition", primerAtaque);
        var primerAtaquePerception = new ConditionPerception();
        primerAtaquePerception.onCheck = PrimerAtaque;
        primerAtaqueCondition.SetPerception(primerAtaquePerception);

        var ataqueFuerte_action = new FunctionalAction();
        ataqueFuerte_action.onStarted = StartAtaqueFuerte;
        ataqueFuerte_action.onUpdated = UpdateAtaqueFuerte;
        var ataqueFuerte = TanqueAtaques.CreateLeafNode("ataqueFuerte", ataqueFuerte_action);

        var ataqueBasico2_action = new FunctionalAction();
        ataqueBasico2_action.onStarted = StartAtaqueBasico;
        ataqueBasico2_action.onUpdated = UpdateAtaqueBasico;
        var ataqueBasico2 = TanqueAtaques.CreateLeafNode("ataqueBasico2", ataqueBasico2_action);

        var secuenciaAtaques = TanqueAtaques.CreateComposite<SequencerNode>("secuenciaAtaques", false, ataqueFuerte, ataqueBasico2);
        secuenciaAtaques.IsRandomized = false;

        var ataquesBasicos_action = new FunctionalAction();
        ataquesBasicos_action.onStarted = StartAtaquesBasicos;
        ataquesBasicos_action.onUpdated = UpdateAtaquesBasicos;
        var ataquesBasicos = TanqueAtaques.CreateLeafNode("ataquesBasicos", ataquesBasicos_action);

        var seleccionAtaques3 = TanqueAtaques.CreateComposite<SelectorNode>("seleccionAtaques3", true, secuenciaAtaques, ataquesBasicos);
        seleccionAtaques3.IsRandomized = true;

        var mitadVidaCondition = TanqueAtaques.CreateDecorator<ConditionNode>("mitadVidaCondition", seleccionAtaques3);
        var mitadVidaPerception = new ConditionPerception();
        mitadVidaPerception.onCheck = MitadVida;
        mitadVidaCondition.SetPerception(mitadVidaPerception);

        var ataqueBasico1_action = new FunctionalAction();
        ataqueBasico1_action.onStarted = StartAtaqueBasico;
        ataqueBasico1_action.onUpdated = UpdateAtaqueBasico;
        var ataqueBasico1 = TanqueAtaques.CreateLeafNode("ataqueBasico1", ataqueBasico1_action);

        var seleccionAtaques2 = TanqueAtaques.CreateComposite<SelectorNode>("seleccionAtaques2", false, mitadVidaCondition, ataqueBasico1);
        seleccionAtaques2.IsRandomized = false;

        var seleccionAtaques = TanqueAtaques.CreateComposite<SelectorNode>("seleccionAtaques", false, primerAtaqueCondition, seleccionAtaques2);
        seleccionAtaques.IsRandomized = false;

        var idle_Invocado_action = new FunctionalAction();
        idle_Invocado_action.onStarted = StartIdleAction;
        idle_Invocado_action.onUpdated = UpdateIdleAction;
        var idle_Invocado = TanqueInvocado.CreateState("idle_Invocado", idle_Invocado_action);

        var Perseguir_action = new FunctionalAction();
        Perseguir_action.onStarted = StartPerseguir;
        Perseguir_action.onUpdated = UpdatePerseguir;
        var Perseguir = TanqueInvocado.CreateState("Perseguir", Perseguir_action);

        var AtacarBasico_action = new FunctionalAction();
        AtacarBasico_action.onStarted = StartAtaqueBasico;
        AtacarBasico_action.onUpdated = UpdateAtaqueBasico;
        var AtacarBasico = TanqueInvocado.CreateState("AtacarBasico", AtacarBasico_action);

        var jugadorCerca_perception = new ConditionPerception();
        jugadorCerca_perception.onCheck = onObjective;
        var jugadorCerca = TanqueInvocado.CreateTransition("jugadorCerca", Perseguir, AtacarBasico, jugadorCerca_perception);

        var ataqueTerminado_perception = new ConditionPerception();
        ataqueTerminado_perception.onCheck = AttackFinished;
        var ataqueTerminado = TanqueInvocado.CreateTransition("ataqueTerminado", AtacarBasico, Perseguir, ataqueTerminado_perception);

        var AtacarFuerte_action = new FunctionalAction();
        AtacarFuerte_action.onStarted = StartAtaqueFuerte;
        AtacarFuerte_action.onUpdated = UpdateAtaqueFuerte;
        var AtacarFuerte = TanqueInvocado.CreateState("AtacarFuerte", AtacarFuerte_action);

        var ataqueFuerteTerminado_perception = new ConditionPerception();
        ataqueFuerteTerminado_perception.onCheck = AttackFinished;
        var ataqueFuerteTerminado = TanqueInvocado.CreateTransition("ataqueFuerteTerminado", AtacarFuerte, Perseguir, ataqueFuerteTerminado_perception);

        var jugadorMuyCerca_perception = new ConditionPerception();
        jugadorMuyCerca_perception.onCheck = onObjectiveClose;
        var jugadorMuyCerca = TanqueInvocado.CreateTransition("jugadorMuyCerca", Perseguir, AtacarFuerte, jugadorMuyCerca_perception);

        var objetivoEncontrado_perception = new ConditionPerception();
        objetivoEncontrado_perception.onCheck = playerClose;
        var objetivoEncontrado = TanqueInvocado.CreateTransition("objetivoEncontrado", idle_Invocado, Perseguir, objetivoEncontrado_perception);

        Tanque.SetRootNode(selector);
        TanqueAtaques.SetRootNode(seleccionAtaques);

        _debugger.RegisterGraph(Tanque);
        _debugger.RegisterGraph(Tanque_1);
        _debugger.RegisterGraph(TanqueAtaques);
        _debugger.RegisterGraph(TanqueInvocado);

        return Tanque;
    }

    private bool MitadVida()
    {
        if(_enemy.Hp.Value <= halfHp)
        {
            mitadVida = true;
        }

        return mitadVida;
    }

    private bool PrimerAtaque()
    {
        return primerAtaque;
    }

    private bool NoInvocado()
    {
        return !invocado;
    }

    private bool Invocado()
    {
        return invocado;
    }


    private void StartIdleAction()
    {
        // Lo que se quiera hacer en el idle, como si es nada
    }

    private Status UpdateIdleAction()
    {
        return Status.Success;
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
        ataqueFinalizado = false;
        return atacarPlayerFar;
    }

    private Boolean AttackFinished()
    {
        return ataqueFinalizado;
    }

    private void StartAtaqueBasico()
    {
        primerAtaque = false;
        ataqueFinalizado = false;
        AnimacionAtacar();
    }

    private Status UpdateAtaqueBasico()
    {
        // nada que añadir

        ataqueFinalizado = true;
        return Status.Success;
    }

    private void StartAtaqueFuerte()
    {
        ataqueFinalizado = false;
        AnimacionAtacar();
    }

    private Status UpdateAtaqueFuerte()
    {
        // nada que añadir

        ataqueFinalizado = true;
        return Status.Success;
    }

    private void StartAtaquesBasicos()
    {
        // Secuencia de 2 ataques basicos
        ataqueFinalizado = false;

        AnimacionAtacar();
        Invoke("AnimacionAtacar", 1f);

    }
    public void AnimacionAtacar()
    {

        _animator.SetTrigger("Attack");
    }

    public void DealDMGPlayer(int dmg)
    {

        _pC.TakeDamage(dmg);
    }
    private Status UpdateAtaquesBasicos()
    {

        ataqueFinalizado = true;
        return Status.Running;
    }

    private Boolean onObjectiveClose()
    {
        ataqueFinalizado = false;
        return atacarPlayerClose;
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
}
