using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class MyTestBezier : MonoBehaviour
{
	[SerializeField]
	private List<RectTransform> _pointTrans;

	private List<Vector2> pointTrans;
	public List<Vector2> PointTrans
	{
		get
		{
			if(null == pointTrans)
			{
				pointTrans = new List<Vector2>();
				pointTrans.Add(_pointTrans[0].anchoredPosition);
				pointTrans.Add(new Vector2(0, 0));
				Vector2 tOutPoint = Vector2.zero;
				RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)GameUI.Instance.MainPanelCanvas.transform,
					Input.mousePosition, GameUI.Instance.UICamera, out tOutPoint);
				pointTrans.Add(tOutPoint);
			}
			return pointTrans;
		}
	}


	public List<Vector2> track=new List<Vector2>();     //最终轨迹
	public GameObject Ball;
	private float sumTime;          //当前累加的时间
	public float timeLimit = 2f;    //重复的时间长度
	private int i = 1;
	[Range(0, 100)]
	[SerializeField]
	private int curveCount = 20;
	[Range(0, 200f)]
	[SerializeField]
	private float lerpSize = 100f;
	[SerializeField]
	private LineRenderer lineRenderer;

	
	public static List<Vector2> GetBezierPath(List<Vector2> _sourcePoints, int _curveCount, float _lerpSize)
	{
		List<Vector2> tResultPath = new List<Vector2>();

		for(int i = 0; i < _curveCount; i++)
		{
			List<Vector2> tResultList = GetVectorCopy(_sourcePoints);
			List<Vector2> tList = new List<Vector2>();
			while(tResultList.Count > 1)
			{
				tList.Clear();
				for(int j = 0; j < tResultList.Count - 1; j++)
				{
					Vector2 tPoint = Vector2.Lerp(tResultList[j], tResultList[j + 1], i / _lerpSize);
					tList.Add(tPoint);
				}
				tResultList = GetVectorCopy(tList);
			}
			tResultPath.Add(tResultList[0]);
		}
		return tResultPath;
	}
	public static List<Vector3> GetBezierPath(List<Vector3> _sourcePoints, int _curveCount, float _lerpSize)
	{
		List<Vector3> tResultPath = new List<Vector3>();

		for(int i = 0; i < _curveCount; i++)
		{
			List<Vector3> tResultList = GetVectorCopy(_sourcePoints);
			List<Vector3> tList = new List<Vector3>();
			while(tResultList.Count > 1)
			{
				tList.Clear();
				for(int j = 0; j < tResultList.Count - 1; j++)
				{
					Vector3 tPoint = Vector3.Lerp(tResultList[j], tResultList[j + 1], i / _lerpSize);
					tList.Add(tPoint);
				}
				tResultList = GetVectorCopy(tList);
			}
			tResultPath.Add(tResultList[0]);
		}
		return tResultPath;
	}

	private void refreshTrack()         //刷新曲线
	{
		track.Clear();
		for(int i = 0; i < curveCount; i++)
		{
			List<Vector2> tResultList = GetVectorCopy(PointTrans);
			List<Vector2> tList = new List<Vector2>();
			while(tResultList.Count > 1)
			{
				tList.Clear();
				for(int j = 0; j < tResultList.Count - 1; j++)
				{
					Vector2 tPoint = Vector2.Lerp(tResultList[j], tResultList[j + 1], i / lerpSize);
					tList.Add(tPoint);
				}
				tResultList = GetVectorCopy(tList);
			}
			track.Add(tResultList[0]);
		}
		//track.Insert(0, _pointTrans[0].anchoredPosition);
		//Vector2 tOutPoint = Vector2.zero;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)GameUI.Instance.MainPanelCanvas.transform,
		//	Input.mousePosition, GameUI.Instance.UICamera, out tOutPoint);
		//track.Add(tOutPoint);
	}
	private void refreshInitialPoints()		//刷新初始点
	{
		pointTrans.Clear();
		pointTrans.Add(_pointTrans[0].anchoredPosition);
		pointTrans.Add(new Vector2(0, 0));
		Vector2 tOutPoint = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)GameUI.Instance.MainPanelCanvas.transform,
			Input.mousePosition, GameUI.Instance.UICamera, out tOutPoint);
		pointTrans.Add(tOutPoint);
	}
	void Awake()
	{
		refreshTrack();
	}

	void OnDrawGizmos()//画线
	{
		Gizmos.color = Color.yellow;
		for(int i = 0; i < track.Count - 1; i++)
		{
			//Gizmos.DrawLine(track[i], track[i + 1]);
		}
	}
	private void lineRendererDraw()
	{
		Vector2[] tVector2s = track.ToArray();
		Vector3[] tVector3s = new Vector3[tVector2s.Length];
		for(int i = 0; i < tVector2s.Length; i++)
		{
			//Vector2 tWorldPoint = transform.InverseTransformPoint(tVector2s[i]);
			tVector3s[i] = new Vector3(tVector2s[i].x / 5, tVector2s[i].y / 5, 0f);
		}
		lineRenderer.positionCount = tVector3s.Length;
		//lineRenderer.startWidth = 0.1f;
		//lineRenderer.endWidth = 0.1f;
		//lineRenderer.transform.position = new Vector3(-tVector3s[0].x, -tVector3s[0].y, 0);
		
		lineRenderer.SetPositions(tVector3s);
	}
	void Update()
	{
		checkPositionIsChanged();
		sumTime += Time.deltaTime;
		if(sumTime > timeLimit)
		{
			sumTime = 0;
		}
		else
		{
			Ball.transform.localPosition = Vector2.Lerp(track[i - 1], track[i], Time.deltaTime * 100);
			Vector2 tDelta = track[i] - track[i - 1];

			i++;
			if(i >= track.Count)
			{
				i = 1;
			}
		}
	}
	public static List<Vector2> GetVectorCopy(List<Vector2> _sourceList) //拷贝vector2 的List
	{
		List<Vector2> tVectorLst = new List<Vector2>();
		for(int i = 0; i < _sourceList.Count; i++)
		{
			tVectorLst.Add(_sourceList[i]);
		}
		return tVectorLst;
	}
	public static List<Vector3> GetVectorCopy(List<Vector3> _sourceList) //拷贝vector3 的List
	{
		List<Vector3> tVectorLst = new List<Vector3>();
		for(int i = 0; i < _sourceList.Count; i++)
		{
			tVectorLst.Add(_sourceList[i]);
		}
		return tVectorLst;
	}
	private void checkPositionIsChanged()       //检测位置是否发生变化
	{
		if(_pointTrans[0].hasChanged || Input.mousePresent)
		{
			refreshInitialPoints();
			refreshTrack();
			lineRendererDraw();
		}
	}
}
