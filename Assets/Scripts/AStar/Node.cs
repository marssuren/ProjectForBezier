using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Node : IComparable       //表示地图的2D网格中的每一个图块对象
{
	public float nodeTotalCost;
	public float estimateCost;
	public Node Parent = null;
	public bool IsObstacle;
	public Vector3 position;

	public Node()
	{
		estimateCost = 0f;
		nodeTotalCost = 1f;
		IsObstacle = false;
		Parent = null;

	}
	public Node(Vector3 _position)
	{
		estimateCost = 0f;
		nodeTotalCost = 1f;
		IsObstacle = false;
		Parent = null;
		position = _position;

	}
	public void MarkAsObstacle()
	{
		IsObstacle = true;
	}

	public int CompareTo(object _obj)
	{
		Node tNode = (Node)_obj;
		//Negative value means object comes before this in the sort order
		if(estimateCost < tNode.estimateCost)
		{
			return -1;
		}
		//Positive value means object comes after this in the sort order
		if(estimateCost > tNode.estimateCost)
		{
			return 1;
		}

		return 0;
	}
}
