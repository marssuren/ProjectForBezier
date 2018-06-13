using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
	private static GameUI instance;

	public static GameUI Instance
	{
		get
		{
			return instance;
		}
	}


	public Camera UICamera;
	public Canvas MainPanelCanvas;
	void Awake()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
