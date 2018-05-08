using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class MyTestBezier : MonoBehaviour
{
	public List<Vector2> PointTrans;
	
	public List<Vector2> track;     //最终轨迹
	public GameObject Ball;
	private float sumTime;          //当前累加的时间
	public float timeLimit = 2f;    //重复的时间长度
    private int i = 1;
	[Range(0, 100)]
	[SerializeField]
	private int curveCount = 100;   
	[Range(0, 200f)]
	[SerializeField]
	private float lerpSize = 100f;
    
	private void init()
	{
		track = new List<Vector2>();
        //PointTrans.Add(new Vector2(-10,0));
        //PointTrans.Add(new Vector2(0,10));
        //PointTrans.Add(new Vector2(10,0));
		for(int i = 0; i < curveCount; i++)
		{
            List<Vector2> tResultList=getVectorCopy(PointTrans);
            List<Vector2> tList = new List<Vector2>();
			while (tResultList.Count>1)
			{
                tList.Clear();
				for(int j = 0; j < tResultList.Count-1; j++)
				{
					Vector2 tPoint=Vector2.Lerp(tResultList[j], tResultList[j+1],i/lerpSize);
                    tList.Add(tPoint);
				}
                tResultList = getVectorCopy(tList);
			}
            track.Add(tResultList[0]);
		}
	}
	void Awake()
	{
		init();
	}
    
	void OnDrawGizmos()//画线
	{
		Gizmos.color = Color.yellow;
		for(int i = 0; i < track.Count - 1; i++)
		{
			Gizmos.DrawLine(track[i], track[i + 1]);
		}
        
	}
	void Update()
	{
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
    private List<Vector2> getVectorCopy(List<Vector2> _sourceList) //拷贝vector2 的List
    {
        List<Vector2> tVectorLst=new List<Vector2>();
        for (int i = 0; i < _sourceList.Count; i++)
        {
            tVectorLst.Add(_sourceList[i]);
        }
        return tVectorLst;
    }
    private List<Vector3> getVectorCopy(List<Vector3> _sourceList) //拷贝vector3 的List
    {
        List<Vector3> tVectorLst = new List<Vector3>();
        for (int i = 0; i < _sourceList.Count; i++)
        {
            tVectorLst.Add(_sourceList[i]);
        }
        return tVectorLst;
    }
}
