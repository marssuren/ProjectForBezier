using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogDebug
{
	public static void Log(string _str)
	{
		if (Global.IsDebugMode)
		{
			UnityEngine.Debug.Log(_str);
		}
	}

}
