using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;

public class ExplosivoBehaviour : BehaviourRunner
{
	
	
	protected override BehaviourGraph CreateGraph()
	{
		var Explosivo = new FSM();
		
		var Dormido = Explosivo.CreateState("Dormido");
		
		var Persiguiendo_action = new FunctionalAction();
		Persiguiendo_action.onStarted = StartPersiguiendo;
		Persiguiendo_action.onUpdated = UpdatePersiguiendo;
		var Persiguiendo = Explosivo.CreateState("Persiguiendo", Persiguiendo_action);
		
		var JugadorDetectado_perception = new ConditionPerception();
		JugadorDetectado_perception.onCheck = playerClose;
		var JugadorDetectado = Explosivo.CreateTransition("JugadorDetectado", Dormido, Persiguiendo, JugadorDetectado_perception);
		
		var Explotar_action = new SimpleAction();
		Explotar_action.action = explode;
		var Explotar = Explosivo.CreateState("Explotar", Explotar_action);
		
		var JugadorCerca_perception = new ConditionPerception();
		JugadorCerca_perception.onCheck = onObjective;
		var JugadorCerca_action = new SimpleAction();
		JugadorCerca_action.action = prepareExplode;
		var JugadorCerca = Explosivo.CreateTransition("JugadorCerca", Persiguiendo, Explotar, JugadorCerca_perception, JugadorCerca_action);
		
		var entrarExplotar_perception = new UnityTimePerception();
		entrarExplotar_perception.TotalTime = 60f;
		var entrarExplotar = Explosivo.CreateTransition("entrarExplotar", Persiguiendo, Explotar, entrarExplotar_perception);
		
		var Moverse_action = new FunctionalAction();
		Moverse_action.onStarted = StartMoverse;
		Moverse_action.onUpdated = UpdateMoverse;
		var Moverse = Explosivo.CreateState("Moverse", Moverse_action);
		
		var Despertar_perception = new UnityTimePerception();
		Despertar_perception.TotalTime = 120f;
		var Despertar = Explosivo.CreateTransition("Despertar", Dormido, Moverse, Despertar_perception);
		
		var JugadorVisto_perception = new ConditionPerception();
		JugadorVisto_perception.onCheck = playerSeen;
		var JugadorVisto = Explosivo.CreateTransition("JugadorVisto", Moverse, Persiguiendo, JugadorVisto_perception);
		
		return Explosivo;
	}
	
	private void StartPersiguiendo()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdatePersiguiendo()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerClose()
	{
		throw new System.NotImplementedException();
	}
	
	private void explode()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean onObjective()
	{
		throw new System.NotImplementedException();
	}
	
	private void prepareExplode()
	{
		throw new System.NotImplementedException();
	}
	
	private void StartMoverse()
	{
		throw new System.NotImplementedException();
	}
	
	private Status UpdateMoverse()
	{
		throw new System.NotImplementedException();
	}
	
	private Boolean playerSeen()
	{
		throw new System.NotImplementedException();
	}
}
