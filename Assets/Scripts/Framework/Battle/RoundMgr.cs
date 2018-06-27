using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMgr
{
	private int rounds;
	private bool isPlayersTurn;
	public bool IsPlayersTurn
	{
		get
		{
			return isPlayersTurn;
		}
	}
	void Awake()
	{
		Events.AddEvent(EventSign.ROUND_END, onRoundStart, this);
	}
	private void onRoundStart(EventArg _eventArg)
	{
		rounds++;
		isPlayersTurn = !isPlayersTurn;

	}


}
