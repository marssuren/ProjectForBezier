using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue : MonoBehaviour
{

	private List<Node> nodes = new List<Node>();
	public int Length
	{
		get
		{
			return nodes.Count;
		}
	}
	public bool Contains(Node _node)
	{
		return nodes.Contains(_node);
	}
	public Node First()
	{
		if(nodes.Count > 0)
		{
			return nodes[0];
		}
		return null;
	}
	public void Push(Node _node)
	{
		nodes.Add(_node);
		nodes.Sort();
	}
	public void Remove(Node _node)
	{
		nodes.Remove(_node);
		nodes.Sort();			//Ensure the nodes are sorted
	}
}
