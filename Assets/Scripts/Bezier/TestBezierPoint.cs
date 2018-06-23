using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBezierPoint : MonoBehaviour
{
	[SerializeField]
	private Vector3 frontControlPoint;
	public Vector3 FrontControlPoint
	{
		get
		{
			return frontControlPoint;
		}
		set
		{
			frontControlPoint = value;
		}
	}

}
