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

public class MinionVoladorBehaviour : BehaviourRunner
{
	[SerializeField] private Transform outRange_perception_OtherTransform;
	
	
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
		
		var onRange = MinionVolador.CreateTransition("onRange", Avanzando, Atacando, statusFlags: StatusFlags.Success);
		
		var outRange_perception = new DistancePerception();
		outRange_perception.OtherTransform = outRange_perception_OtherTransform;
		outRange_perception.MaxDistance = 10f;
		var outRange = MinionVolador.CreateTransition("outRange", Atacando, Avanzando, outRange_perception, statusFlags: StatusFlags.Finished);
		
		var PrimerPunto_action = new FunctionalAction();
		PrimerPunto_action.onStarted = StartPrimer;
		PrimerPunto_action.onUpdated = UpdatePrimer;
		var PrimerPunto = MinionVoladorPatrullar.CreateLeafNode("PrimerPunto", PrimerPunto_action);
		
		var SegundoPunto_action = new FunctionalAction();
		SegundoPunto_action.onStarted = StartSegundo;
		SegundoPunto_action.onUpdated = UpdateSegundo;
		var SegundoPunto = MinionVoladorPatrullar.CreateLeafNode("SegundoPunto", SegundoPunto_action);
		
		var TercerPunto_action = new FunctionalAction();
		TercerPunto_action.onStarted = StartPrimer;
		TercerPunto_action.onUpdated = UpdateSegundo;
		var TercerPunto = MinionVoladorPatrullar.CreateLeafNode("TercerPunto", TercerPunto_action);
		
		var Secuencia = MinionVoladorPatrullar.CreateComposite<SequencerNode>("Secuencia", false, PrimerPunto, SegundoPunto, TercerPunto);
		Secuencia.IsRandomized = false;
		
		var Iteraciones = MinionVoladorPatrullar.CreateDecorator<LoopNode>("Iteraciones", Secuencia);
		Iteraciones.Iterations = -1;
		
		return MinionVolador;
	}
	
	private void AvanzandoStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status AvanzandoUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerSeen()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean beingAttacked()
	{
		throw new System.NotImplementedException();
	}
	
	private void AtacarStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status AtacarUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void stunned()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerLost()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean beingStunned()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartPrimer()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdatePrimer()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartSegundo()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateSegundo()
	{
		throw new System.NotImplementedException();
	}
}
