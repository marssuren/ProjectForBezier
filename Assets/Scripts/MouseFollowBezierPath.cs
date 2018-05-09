using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowBezierPath : MonoBehaviour
{
	private bool isWaitingForSecondPoint;
	private List<Vector2> sourcePoints;
	private Vector2 middlePoint = Vector2.zero;
	void Update()
	{


	}
	void OnMouseDown()
	{
		if(!isWaitingForSecondPoint)
		{

			sourcePoints.Clear();
			Vector2 tPoint = new Vector2(transform.position.x,transform.position.y);
			sourcePoints.Add(tPoint);
			sourcePoints.Add(middlePoint);
			isWaitingForSecondPoint = true;
		}
	}
}
