using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.BehaviourTrees;
using BehaviourAPI.StateMachines;

public class TanqueBehaviour : BehaviourRunner
{
	
	
	protected override BehaviourGraph CreateGraph()
	{
		var Tanque = new BehaviourTree();
		var Tanque_1 = new FSM();
		var TanqueAtaques = new BehaviourTree();
		var TanqueInvocado = new FSM();
		
		var NoInvocado_action = new SubsystemAction(Tanque_1, true, ExecutionInterruptOptions.None);
		var NoInvocado = Tanque.CreateLeafNode("NoInvocado", NoInvocado_action);
		
		var NoInvocado_1 = Tanque.CreateDecorator<ConditionNode>("NoInvocado", NoInvocado);
		
		var succeder1 = Tanque.CreateDecorator<SuccederNode>("succeder1", NoInvocado_1);
		
		var Invocado_action = new SubsystemAction(TanqueInvocado, true, ExecutionInterruptOptions.None);
		var Invocado = Tanque.CreateLeafNode("Invocado", Invocado_action);
		
		var Invocado_1 = Tanque.CreateDecorator<ConditionNode>("Invocado", Invocado);
		
		var succeder2 = Tanque.CreateDecorator<SuccederNode>("succeder2", Invocado_1);
		
		var Selector = Tanque.CreateComposite<SelectorNode>("Selector", false, succeder1, succeder2);
		Selector.IsRandomized = false;
		
		var Idle = Tanque_1.CreateState("Idle");
		
		var Perseguir_action = new FunctionalAction();
		Perseguir_action.onStarted = StartPerseguir;
		Perseguir_action.onUpdated = UpdatePerseguir;
		var Perseguir = Tanque_1.CreateState("Perseguir", Perseguir_action);
		
		var jugadorEncontrado_perception = new ConditionPerception();
		jugadorEncontrado_perception.onCheck = playerClose;
		var jugadorEncontrado = Tanque_1.CreateTransition("jugadorEncontrado", Idle, Perseguir, jugadorEncontrado_perception);
		
		var jugadorPerdido_perception = new ConditionPerception();
		jugadorPerdido_perception.onCheck = playerLost;
		var jugadorPerdido = Tanque_1.CreateTransition("jugadorPerdido", Perseguir, Idle, jugadorPerdido_perception);
		
		var Atacar_action = new SubsystemAction(TanqueAtaques);
		var Atacar = Tanque_1.CreateState("Atacar", Atacar_action);
		
		var entrarAtacar_perception = new ConditionPerception();
		entrarAtacar_perception.onCheck = onObjective;
		var entrarAtacar = Tanque_1.CreateTransition("entrarAtacar", Perseguir, Atacar, entrarAtacar_perception);
		
		var salirAtacar = Tanque_1.CreateTransition("salirAtacar", Atacar, Perseguir, statusFlags: StatusFlags.Success);
		
		var PrimerAtaque_1_action = new FunctionalAction();
		PrimerAtaque_1_action.onStarted = StartAtaqueBasico;
		PrimerAtaque_1_action.onUpdated = UpdateAtaqueBasico;
		var PrimerAtaque_1 = TanqueAtaques.CreateLeafNode("PrimerAtaque", PrimerAtaque_1_action);
		
		var PrimerAtaque = TanqueAtaques.CreateDecorator<ConditionNode>("PrimerAtaque", PrimerAtaque_1);
		
		var succederAtaques = TanqueAtaques.CreateDecorator<SuccederNode>("succederAtaques", PrimerAtaque);
		
		var AtaqueFuerte_action = new FunctionalAction();
		AtaqueFuerte_action.onStarted = StartAtaqueFuerte;
		AtaqueFuerte_action.onUpdated = UpdateAtaqueFuerte;
		var AtaqueFuerte = TanqueAtaques.CreateLeafNode("AtaqueFuerte", AtaqueFuerte_action);
		
		var AtaqueBasico_action = new FunctionalAction();
		AtaqueBasico_action.onStarted = StartAtaqueBasico;
		AtaqueBasico_action.onUpdated = UpdateAtaqueBasico;
		var AtaqueBasico = TanqueAtaques.CreateLeafNode("AtaqueBasico", AtaqueBasico_action);
		
		var secuenciaAtaques = TanqueAtaques.CreateComposite<SequencerNode>("secuenciaAtaques", false, AtaqueFuerte, AtaqueBasico);
		secuenciaAtaques.IsRandomized = false;
		
		var AtaquesBasicos_action = new FunctionalAction();
		AtaquesBasicos_action.onStarted = StartAtaquesBasicos;
		AtaquesBasicos_action.onUpdated = UpdateAtaquesBasicos;
		var AtaquesBasicos = TanqueAtaques.CreateLeafNode("AtaquesBasicos", AtaquesBasicos_action);
		
		var Seleccion3 = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion3", true, secuenciaAtaques, AtaquesBasicos);
		Seleccion3.IsRandomized = true;
		
		var mitadVida = TanqueAtaques.CreateDecorator<ConditionNode>("mitadVida", Seleccion3);
		
		var succederAtaques2 = TanqueAtaques.CreateDecorator<SuccederNode>("succederAtaques2", mitadVida);
		
		var AtaqueIndividual_action = new FunctionalAction();
		AtaqueIndividual_action.onStarted = StartAtaqueIndividual;
		AtaqueIndividual_action.onUpdated = UpdateAtaqueIndividual;
		var AtaqueIndividual = TanqueAtaques.CreateLeafNode("AtaqueIndividual", AtaqueIndividual_action);
		
		var Seleccion2 = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion2", false, succederAtaques2, AtaqueIndividual);
		Seleccion2.IsRandomized = false;
		
		var Seleccion = TanqueAtaques.CreateComposite<SelectorNode>("Seleccion", false, succederAtaques, Seleccion2);
		Seleccion.IsRandomized = false;
		
		var Perseguir_1_action = new FunctionalAction();
		Perseguir_1_action.onStarted = StartPerseguir;
		Perseguir_1_action.onUpdated = UpdatePerseguir;
		var Perseguir_1 = TanqueInvocado.CreateState("Perseguir", Perseguir_1_action);
		
		var AtacarBasico_action = new FunctionalAction();
		AtacarBasico_action.onStarted = StartAtacar;
		AtacarBasico_action.onUpdated = UpdateAtacar;
		var AtacarBasico = TanqueInvocado.CreateState("AtacarBasico", AtacarBasico_action);
		
		var jugadorCerca_perception = new ConditionPerception();
		jugadorCerca_perception.onCheck = onObjective;
		var jugadorCerca = TanqueInvocado.CreateTransition("jugadorCerca", Perseguir_1, AtacarBasico, jugadorCerca_perception);
		
		var ataqueTerminado = TanqueInvocado.CreateTransition("ataqueTerminado", AtacarBasico, Perseguir_1, statusFlags: StatusFlags.Success);
		
		var AtacarFuerte_action = new FunctionalAction();
		AtacarFuerte_action.onStarted = StartAtacarFuerte;
		AtacarFuerte_action.onUpdated = UpdateAtacarFuerte;
		var AtacarFuerte = TanqueInvocado.CreateState("AtacarFuerte", AtacarFuerte_action);
		
		var ataqueFuerteTerminado = TanqueInvocado.CreateTransition("ataqueFuerteTerminado", AtacarFuerte, Perseguir_1, statusFlags: StatusFlags.Success);
		
		var jugadorMuyCerca_perception = new ConditionPerception();
		jugadorMuyCerca_perception.onCheck = onObjectiveClose;
		var jugadorMuyCerca = TanqueInvocado.CreateTransition("jugadorMuyCerca", Perseguir_1, AtacarFuerte, jugadorMuyCerca_perception);
		
		return Tanque;
	}
	
	private void StartPerseguir()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdatePerseguir()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerClose()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerLost()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean onObjective()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartAtaqueBasico()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtaqueBasico()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartAtaqueFuerte()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtaqueFuerte()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartAtaquesBasicos()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtaquesBasicos()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartAtaqueIndividual()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtaqueIndividual()
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
	
	private void StartAtacarFuerte()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateAtacarFuerte()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean onObjectiveClose()
	{
		throw new System.NotImplementedException();
	}
}
