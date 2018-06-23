using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceFSM : MonoBehaviour
{
	private List<FSMState> fsmStates;       //存储FSMState对象
	private FSMStateID currentStateId;      //存储FSMStateID
	public FSMStateID CurrenStateId
	{
		get
		{
			return currentStateId;
		}
	}

	private FSMState currentState;          //存储当前FSMState
	public FSMState CurrentState
	{
		get
		{
			return currentState;
		}
	}
	//AddFSMState和DeleteFSMState用来添加/删除列表中FSMState类的实体。在调用PerformTransition方法时，它会根据相应的转移来更新CurrentState变量的状态
	public void AddFSMState(FSMState _fsmState)
	{
		if(!fsmStates.Contains(_fsmState))
		{
			fsmStates.Add(_fsmState); 
		}
		else
		{
			Debug.Log("Causion!FSMState Repeated!!!");
		}
	}
	public void DeleteFSMState(FSMState _fsmState)
	{
		if (fsmStates.Contains(_fsmState))
		{
			fsmStates.Remove(_fsmState);
		}
	}

	void Update()
	{
		//currentState.Reason();
		//currentState.Act();

	}
}
public enum Transition
{
	None = 0,
	SawPlayer = 1,
	ReachPlayer = 2,
	LostPlayer = 3,
	NoHealth = 4,
}
public enum FSMStateID
{
	None = 0,
	Patrolling = 1,
	Chasing = 2,
	Attacking = 3,
	Dead = 4,

}
