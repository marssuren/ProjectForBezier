using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;


//public delegate void Callback();


public class EventStruct/*where T :new()*/
{
	private List<Action<EventArg>> callbackLst;
	public List<Action<EventArg>> CallbackLst
	{
		get
		{
			if(null == callbackLst)
			{
				callbackLst = new List<Action<EventArg>>();
			}

			return callbackLst;
		}
	}
	


	private string key;
	public string Key
	{
		get
		{
			return key;
		}
	}

	//private Delegate funcLst;
	//public Delegate FuncLst
	//{
	//	get { return funcLst; }
	//}


	private Object source;
	public Object Source
	{
		get
		{
			return source;
		}
	}

	


	//public callbacks Callbacks;

	//private List<Delegate> funcLst;
	//public List<Delegate> FuncLst
	//{
	//	get
	//	{
	//		if(null == funcLst)
	//		{
	//			funcLst = new List<Delegate>();
	//		}
	//		return funcLst;
	//	}
	//}
	public EventStruct(string _key, Object _source)
	{
		key = _key;
		source = _source;
	}
}
