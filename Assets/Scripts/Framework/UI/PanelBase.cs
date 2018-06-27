using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
	public void Show(EventArg _eventArg=null)
	{

	}
	public void Hide()
	{
		gameObject.SetActive(false);
	}

}
