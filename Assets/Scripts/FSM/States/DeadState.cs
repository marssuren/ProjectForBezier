using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState {

	
	public override void Reason(Transform _player, Transform _npc)
	{
		throw new System.NotImplementedException();
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
