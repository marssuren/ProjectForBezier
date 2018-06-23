using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
	private Transform[] wayPoints;
	public ChaseState(Transform[] _wayPoints)
	{
		wayPoints = _wayPoints;
		stateID = FSMStateID.Chasing;
	}
	public override void Reason(Transform _player, Transform _npc)
	{
		if(Vector3.Distance(_player.position, _npc.position) > 300f)
		{
			_npc.GetComponent<NPCController>().SetTransition(Transition.LostPlayer);
		}
		else
		{
			if(Vector3.Distance(_player.position, _npc.position) <= 100f)
			{
				_npc.GetComponent<NPCController>().SetTransition(Transition.ReachPlayer);

			}
		}
	}

	public override void Act(Transform _player, Transform _npc)
	{
		throw new System.NotImplementedException();
	}
	public void AddTransition(Transition _transition, FSMStateID _fsmStateId)
	{
		base.AddTransition(_transition, _fsmStateId);
	}
}
