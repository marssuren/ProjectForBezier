using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
	private Transform[] wayPoints;
	public PatrolState(Transform[] _wayPoints)
	{
		wayPoints = _wayPoints;
		stateID = FSMStateID.Patrolling;
	}
	public override void Reason(Transform _player, Transform _npc)
	{
		if(Vector3.Distance(_player.position, _npc.position) <= 300f)
		{
			_npc.GetComponent<NPCController>().SetTransition(Transition.SawPlayer);
		}
	}
	public override void Act(Transform _player, Transform _npc)
	{
		throw new NotImplementedException();
	}
	public void AddTransition(Transition _transition, FSMStateID _fsmStateId)
	{
		base.AddTransition(_transition,_fsmStateId);
	}




}
