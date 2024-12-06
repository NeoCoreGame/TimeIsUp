using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;

public class EscurridizoBehaviour : BehaviourRunner
{
	[SerializeField] private Transform Huir_action_OtherTransform;
	
	
	protected override BehaviourGraph CreateGraph()
	{
		var Escurridizo = new FSM();
		var EscurridizoPatrullar = new BehaviourTree();
		
		var Patrullar_action = new SubsystemAction(EscurridizoPatrullar, true, ExecutionInterruptOptions.None);
		var Patrullar = Escurridizo.CreateState("Patrullar", Patrullar_action);
		
		var Huir_action = new FleeAction();
		Huir_action.OtherTransform = Huir_action_OtherTransform;
		Huir_action.speed = 0f;
		Huir_action.distance = 0f;
		Huir_action.maxTimeRunning = 0f;
		var Huir = Escurridizo.CreateState("Huir", Huir_action);
		
		var Dash_action = new FunctionalAction();
		Dash_action.onStarted = StartDash;
		Dash_action.onUpdated = UpdateDash;
		var Dash = Escurridizo.CreateState("Dash", Dash_action);
		
		var Dashear_perception = new DistancePerception();
		Dashear_perception.OtherTransform = Huir_action_OtherTransform;
		Dashear_perception.MaxDistance = 0f;
		var Dashear = Escurridizo.CreateTransition("Dashear", Huir, Dash, Dashear_perception);
		
		var Dash_Huir = Escurridizo.CreateTransition("Dash_Huir", Dash, Huir, statusFlags: StatusFlags.Success);
		
		var JugadorVisto_perception = new ConditionPerception();
		JugadorVisto_perception.onCheck = playerClose;
		var JugadorVisto = Escurridizo.CreateTransition("JugadorVisto", Patrullar, Huir, JugadorVisto_perception);
		
		var Atacar_action = new FunctionalAction();
		Atacar_action.onStarted = StartAtacar;
		Atacar_action.onUpdated = UpdateAtacar;
		var Atacar = Escurridizo.CreateState("Atacar", Atacar_action);
		
		var Atacar_1_perception = new ConditionPerception();
		Atacar_1_perception.onCheck = attack;
		var Atacar_1 = Escurridizo.CreateTransition("Atacar", Huir, Atacar, Atacar_1_perception);
		
		var Atacar_Huir = Escurridizo.CreateTransition("Atacar_Huir", Atacar, Huir, statusFlags: StatusFlags.Success);
		
		var PrimerPunto_action = new FunctionalAction();
		PrimerPunto_action.onStarted = PrimerStart;
		PrimerPunto_action.onUpdated = PrimerUpdate;
		var PrimerPunto = EscurridizoPatrullar.CreateLeafNode("PrimerPunto", PrimerPunto_action);
		
		var SegundoPunto_action = new FunctionalAction();
		SegundoPunto_action.onStarted = SegundoStart;
		SegundoPunto_action.onUpdated = SegundoUpdate;
		var SegundoPunto = EscurridizoPatrullar.CreateLeafNode("SegundoPunto", SegundoPunto_action);
		
		var TercerPunto_action = new FunctionalAction();
		TercerPunto_action.onStarted = TercerStart;
		TercerPunto_action.onUpdated = TercerUpdate;
		var TercerPunto = EscurridizoPatrullar.CreateLeafNode("TercerPunto", TercerPunto_action);
		
		var CuartoPunto_action = new FunctionalAction();
		CuartoPunto_action.onStarted = CuartoStart;
		CuartoPunto_action.onUpdated = CuartoUpdate;
		var CuartoPunto = EscurridizoPatrullar.CreateLeafNode("CuartoPunto", CuartoPunto_action);
		
		var Secuencia = EscurridizoPatrullar.CreateComposite<SequencerNode>("Secuencia", false, PrimerPunto, SegundoPunto, TercerPunto, CuartoPunto);
		Secuencia.IsRandomized = false;
		
		var iteraciones = EscurridizoPatrullar.CreateDecorator<LoopNode>("iteraciones", Secuencia);
		iteraciones.Iterations = -1;
		
		return Escurridizo;
	}
	
	private void StartDash()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateDash()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerClose()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartAtacar()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtacar()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean attack()
	{
		throw new System.NotImplementedException();
	}
	
	private void PrimerStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status PrimerUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void SegundoStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status SegundoUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void TercerStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status TercerUpdate()
	{
		throw new System.NotImplementedException();
	}
	
	private void CuartoStart()
	{
		throw new System.NotImplementedException();
	}
	
	private Status CuartoUpdate()
	{
		throw new System.NotImplementedException();
	}
}
