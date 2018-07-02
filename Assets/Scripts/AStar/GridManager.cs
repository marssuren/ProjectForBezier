using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	private static GridManager instance;
	public static GridManager Instance
	{
		get
		{
			if(null == instance)
			{
				instance = FindObjectOfType<GridManager>();
				if(null == instance)
				{
					LogDebug.Log("Could not locate a GridManager");
				}
			}

			return instance;
		}
	}

	public int NumOfRows;
	public int NumOfColumns;
	public float gridCellSize;
	public bool IsShowGrid = true;
	public bool IsShowObstacleBlocks = true;
	private Vector3 origin = new Vector3();
	private GameObject[] obstacleLst;
	public Node[,] Nodes
	{
		get;
		set;
	}
	public Vector3 Origin
	{
		get
		{
			return origin;
		}
	}
	void Awake()
	{
		obstacleLst = GameObject.FindGameObjectsWithTag("Obstacle");
		calculateObstacles();
	}
	private void calculateObstacles()
	{
		Nodes = new Node[NumOfColumns, NumOfRows];
		int tIndex = 0;
		for(int i = 0; i < NumOfColumns; i++)
		{
			for(int j = 0; j < NumOfRows; j++)
			{
				Vector3 tCellPos = GetGridCellCenter(tIndex);
				Node tNode = new Node(tCellPos);
				Nodes[i, j] = tNode;
				tIndex++;
			}
		}

		if(obstacleLst != null && obstacleLst.Length > 0)
		{
			for(int i = 0; i < obstacleLst.Length; i++)
			{
				GameObject tObstacle = obstacleLst[i];
				int tIndexCell = GetGridIndex(tObstacle.transform.position);
				int tCol = GetColumn(tIndexCell);
				int tRow = GetRow(tIndexCell);
				Nodes[tRow, tCol].MarkAsObstacle();

			}
		}
	}
	public Vector3 GetGridCellCenter(int _index)
	{
		Vector3 tCellPosition = GetGridCellPosition(_index);
		tCellPosition.x += (gridCellSize / 2f);
		tCellPosition.z += (gridCellSize / 2f);
		return tCellPosition;
	}
	public Vector3 GetGridCellPosition(int _index)
	{
		int tRow = GetRow(_index);
		int tCol = GetColumn(_index);
		float xPosInGrid = tCol * gridCellSize;
		float zPosInGrid = tRow * gridCellSize;
		return Origin + new Vector3(xPosInGrid, 0f, zPosInGrid);
	}
	public int GetRow(int _index)
	{
		int tRow = _index / NumOfColumns;
		return tRow;
	}
	public int GetColumn(int _index)
	{
		int tColumn = _index % NumOfColumns;
		return tColumn;
	}
	public int GetGridIndex(Vector3 _pos)
	{
		if(!IsInBounds(_pos))
		{
			return -1;
		}

		_pos -= Origin;
		int tCol = (int)(_pos.x / gridCellSize);
		int tRow = (int)(_pos.z / gridCellSize);
		return (tRow * NumOfColumns + tCol);
	}
	public bool IsInBounds(Vector3 _pos)
	{
		float tWidth = NumOfColumns * gridCellSize;
		float tHeight = NumOfRows * gridCellSize;
		return (_pos.x >= Origin.x && _pos.x <= (Origin.x + tWidth) && (_pos.x <= Origin.z + tHeight) && _pos.z >= Origin.z);
	}
	public void GetNeighbours(Node _node, List<Node> _neighbours)
	{
		Vector3 tNeighboursPos = _node.position;
		int tNeighbourIndex = GetGridIndex(tNeighboursPos);
		int tRow = GetRow(tNeighbourIndex);
		int tColumn = GetColumn(tNeighbourIndex);
		//Bottom
		int tLeftNodeRow = tRow - 1;
		int tLeftNodeColumn = tColumn;
		assignNeighbour(tLeftNodeRow, tLeftNodeColumn, _neighbours);
		//Top
		tLeftNodeRow = tRow + 1;
		tLeftNodeColumn = tColumn;
		assignNeighbour(tLeftNodeRow, tLeftNodeColumn, _neighbours);
		//Right
		tLeftNodeRow = tRow;
		tLeftNodeColumn = tColumn + 1;
		assignNeighbour(tLeftNodeRow, tLeftNodeColumn, _neighbours);
		//Left
		tLeftNodeRow = tRow;
		tLeftNodeColumn = tColumn - 1;
		assignNeighbour(tLeftNodeRow, tLeftNodeColumn, _neighbours);
	}
	private void assignNeighbour(int _row, int _column, List<Node> _neighbours)
	{
		if(_row != -1 && _column != -1 && _row < NumOfRows && _column < NumOfColumns)
		{
			Node tNode2Add = Nodes[_row, _column];
			if(!tNode2Add.IsObstacle)
			{
				_neighbours.Add(tNode2Add);
			}
		}
	}
	void OnDrawGizmos()
	{
		if(IsShowGrid)
		{
			DebugDrawGrid(transform.position, NumOfRows, NumOfColumns, gridCellSize, Color.blue);
		}
		Gizmos.DrawSphere(transform.position, 0.5f);
		if(IsShowObstacleBlocks)
		{
			Vector3 tCellSize = new Vector3(gridCellSize, 1, gridCellSize);
			if(obstacleLst != null && obstacleLst.Length > 0)
			{
				for(int i = 0; i < obstacleLst.Length; i++)
				{
					GameObject tGo = obstacleLst[i];
					Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(tGo.transform.position)), tCellSize);
				}
			}
		}
	}
	public void DebugDrawGrid(Vector3 _origin, int _numRows, int _numCols, float _cellSize, Color _color)
	{
		float tWidth = (_numCols * _cellSize);
		float tHeight = (_numRows * _cellSize);
		//Draw the horizontal grid lines
		for(int i = 0; i < _numRows + 1; i++)
		{
			Vector3 tStartPos = _origin + i * _cellSize * new Vector3(0, 0, 1);
			Vector3 tEndPos = tStartPos + tWidth * new Vector3(1, 0, 0);
			Debug.DrawLine(tStartPos, tEndPos, _color);
		}
		//Draw the vertical grid lines
		for(int i = 0; i < _numCols + 1; i++)
		{
			Vector3 tStartPos = _origin + i * _cellSize * new Vector3(1, 0, 0);
			Vector3 tEndPos = tStartPos + tHeight * new Vector3(0, 0, 1);
			Debug.DrawLine(tStartPos, tEndPos, _color);
		}
	}
}
