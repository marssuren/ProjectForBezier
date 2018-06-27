using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventArg
{
	public string ContentStr;
	public Object ContentObj;
	public EventArg(string _content = null, Object _object = null)
	{
		ContentStr = _content;
		ContentObj = _object;
	}
}
