using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{

	private Transform startPos, endPos;

	public Node StartNode
	{
		get;
		set;
	}

	public Node GoalNode
	{
		get;
		set;
	}

	public List<Node> pathLst;
	private GameObject ObjStartCube, ObjEndCube;
	private float elapsedTime = 0f;
	//Interval time between pathFinding
	public float intervalTime = 1f;
	void Start()
	{
		ObjStartCube = GameObject.FindGameObjectWithTag("Start");
		ObjEndCube = GameObject.FindGameObjectWithTag("End");
		pathLst = new List<Node>();
	}
	void Update()
	{
		elapsedTime += Time.deltaTime;
		if(elapsedTime >= intervalTime)
		{
			elapsedTime = 0f;
			FindPath();
		}
	}
	public void FindPath()
	{
		startPos = ObjStartCube.transform;
		endPos = ObjEndCube.transform;
		StartNode = new Node(GridManager.Instance.GetGridCellCenter(GridManager.Instance.GetGridIndex(startPos.position)));
		GoalNode = new Node(GridManager.Instance.GetGridCellCenter(GridManager.Instance.GetGridIndex(endPos.position)));
		pathLst = AStar.FindPath(StartNode, GoalNode);
	}
	void OnDrawGizmos()
	{
		if(null == pathLst)
		{
			return;
		}
		if(pathLst.Count > 0)
		{
			int tIndex = 1;
			for(int i = 0; i < pathLst.Count; i++)
			{
				if(tIndex < pathLst.Count)
				{
					Node tNode = pathLst[i];
					Node tNextNode = pathLst[tIndex];
					Debug.DrawLine(tNode.position, tNextNode.position, Color.green);
					tIndex++;
				}
			}
		}
	}
}
