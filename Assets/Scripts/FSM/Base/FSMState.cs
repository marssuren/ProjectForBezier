using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState : MonoBehaviour
{
	protected Dictionary<Transition,FSMStateID> map=new Dictionary<Transition, FSMStateID>();   //存储所有转移和状态的键值对,如SawPlayer->Chasing ，LostPlayer->Patrolling
	protected FSMStateID stateID;
	protected void AddTransition(Transition _transition,FSMStateID _fsmStateId)		//对map进行添加
	{
		if (!map.ContainsKey(_transition))
		{
			map.Add(_transition,_fsmStateId);
		}
	}
	protected void DeleteTransition(Transition _transition)        //对map进行删除
	{
		if(map.ContainsKey(_transition))
		{
			map.Remove(_transition);
		}
	}
	protected FSMStateID GetOutPutState(Transition _transition)		//返回map中的指定状态
	{
		FSMStateID tFsmStateId=new FSMStateID();
		if (map.ContainsKey(_transition))
		{
			tFsmStateId = map[_transition];
		}
		return tFsmStateId;
	}
	//这两个方法都需要转换NPC和玩家实体的数据
	public abstract void Reason(Transform _player, Transform _npc);	//用于检查当前状态是否需要转换到另一个状态
	public abstract void Act(Transform _player, Transform _npc);	//为currentState变量执行实际的任务，如向目标点移动，追逐并攻击



}
