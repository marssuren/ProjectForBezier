using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
	public static PriorityQueue CloseList, OpenList;
	private static float HeuristicEstimateCost(Node _curNode, Node _goalNode)
	{
		Vector3 tVectorCost = _curNode.position - _goalNode.position;
		return tVectorCost.magnitude;
	}
	public static List<Node> FindPath(Node _start, Node _goal)
	{
		OpenList = new PriorityQueue();
		OpenList.Push(_start);
		_start.nodeTotalCost = 0f;
		_start.estimateCost = HeuristicEstimateCost(_start, _goal);
		CloseList = new PriorityQueue();
		Node tNode = null;
		while(OpenList.Length != 0)
		{
			tNode = OpenList.First();
			//Check if the current node is the goal node
			if(tNode.position == _goal.position)
			{
				return calculatePath(tNode);
			}
			//Create an List to store the neighboring nodes
			List<Node> tNeighbours = new List<Node>();
			GridManager.Instance.GetNeighbours(tNode, tNeighbours);
			for(int i = 0; i < tNeighbours.Count; i++)
			{
				Node tNeighbourNode = tNeighbours[i];
				if(!CloseList.Contains(tNeighbourNode))
				{
					float tCost = HeuristicEstimateCost(tNode, tNeighbourNode);
					float tTotalCost = tNode.nodeTotalCost + tCost;
					float tNeighbourNodeEstCost = HeuristicEstimateCost(tNeighbourNode, _goal);
					tNeighbourNode.nodeTotalCost = tTotalCost;
					tNeighbourNode.Parent = tNode;
					tNeighbourNode.estimateCost = tTotalCost + tNeighbourNodeEstCost;
					if(!OpenList.Contains(tNeighbourNode))
					{
						OpenList.Push(tNeighbourNode);
					}
				}
			}
			//Push the current node to the closed list
			CloseList.Push(tNode);
			//and remove it from OpenList
			OpenList.Remove(tNode);
		}
		if(tNode.position != _goal.position)
		{
			Debug.LogError("Goal Not Found!");
			return null;
		}
		return calculatePath(tNode);

	}
	private static List<Node> calculatePath(Node _node)
	{
		List<Node> tList = new List<Node>();
		while(null != _node)
		{
			tList.Add(_node);
			_node = _node.Parent;
		}
		tList.Reverse();
		return tList;
	}
}
