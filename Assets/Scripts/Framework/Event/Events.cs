using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;


public static class Events/* where T:new()*/
{
	private static Dictionary<string, EventStruct> eventsMap = new Dictionary<string, EventStruct>();
	public static void AddEvent(string _key, Action<EventArg> _event, Object _source)
	{
		EventStruct tEventStruct;
		if(eventsMap.ContainsKey(_key))
		{
			tEventStruct = null/*GetEventById(_key)*/;
			if(tEventStruct.CallbackLst.Contains(_event))
			{
				LogDebug.Log("重复添加事件");
				return;
			}
			tEventStruct.CallbackLst.Add(_event);
			return;
		}
		tEventStruct = new EventStruct(_key, _source);
		tEventStruct.CallbackLst.Add(_event);
		eventsMap.Add(_key, tEventStruct);
	}
	public static void Send(string _key, EventArg _arg=null)
	{
		if(eventsMap.ContainsKey(_key))
		{
			EventStruct tEventStruct = eventsMap[_key];
			for(int i = 0; i < tEventStruct.CallbackLst.Count; i++)
			{
				tEventStruct.CallbackLst[i].DynamicInvoke(_arg);
			}
		}
	}
	public static void RmEvent(string _key, Action<EventArg> _event)
	{
		if(eventsMap.ContainsKey(_key))
		{
			if(eventsMap[_key].CallbackLst.Contains(_event))
			{
				eventsMap[_key].CallbackLst.Remove(_event);
			}
		}
		else
		{
			LogDebug.Log("Event Key Not Exist!!!");
		}
	}
	public static void ClearAll()
	{
		eventsMap.Clear();
	}
	public static EventStruct GetEventByKey(string _key)
	{
		EventStruct tEventStruct = null;
		if(eventsMap.ContainsKey(_key))
		{
			tEventStruct = eventsMap[_key];
		}
		if(null == tEventStruct)
		{
			LogDebug.Log("Event Not Exist!!!");
		}
		return tEventStruct;
	}
}
