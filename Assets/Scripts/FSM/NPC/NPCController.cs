using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : AdvanceFSM
{
	private Transform[] wayPoints;
	public void SetTransition(Transition _transition)
	{
		PerformTransition(_transition);
	}
	public void PerformTransition(Transition _transition)
	{

	}
	private void ConstructFSM()         //初始化拥有哪些状态，并为拥有的状态设置转化和状态ID
	{
		PatrolState tPatrolState = new PatrolState(wayPoints);
		tPatrolState.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
		tPatrolState.AddTransition(Transition.NoHealth, FSMStateID.Dead);

		ChaseState tChaseState = new ChaseState(wayPoints);
		tChaseState.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
		tChaseState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
		tChaseState.AddTransition(Transition.NoHealth, FSMStateID.Dead);

		AttackState tAttackState = new AttackState(wayPoints);
		tAttackState.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
		tAttackState.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
		tAttackState.AddTransition(Transition.NoHealth, FSMStateID.Dead);

		DeadState tDeadState=new DeadState();
		tDeadState.AddTransition(Transition.NoHealth,FSMStateID.Dead);

		AddFSMState(tPatrolState);
		AddFSMState(tPatrolState);
		AddFSMState(tPatrolState);
		AddFSMState(tPatrolState);





	}
}
