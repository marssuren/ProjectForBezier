using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTestBezier : MonoBehaviour
{
    public List<Transform> PointTrans;
    public List<Vector2> track;
    public GameObject Ball;
    private float sumTime ;
    public float timeLimit=2f;
    private int i = 1;
    private void init()
    {
        track=new List<Vector2>();
        for(int i = 0; i < 200; i++)
        {
            Vector2 tPointA1 = Vector2.Lerp(PointTrans[0].position, PointTrans[1].position, i / 100f);
            Vector2 tPointA2 = Vector2.Lerp(PointTrans[1].position, PointTrans[2].position, i / 100f);
            //Vector2 tPointA3 = Vector2.Lerp(PointTrans[2].position, PointTrans[3].position, i / 100f);
            //Vector2 tPointA4 = Vector2.Lerp(PointTrans[3].position, PointTrans[4].position, i / 100f);

            Vector2 tResultPoint = Vector2.Lerp(tPointA1, tPointA2, i / 100f);
            //Vector2 tPointB2 = Vector2.Lerp(tPointA2, tPointA3, i / 100f);
            //Vector2 tPointB3 = Vector2.Lerp(tPointA3, tPointA4, i / 100f);

            //Vector2 tResultPoint = Vector2.Lerp(tPointB1, tPointB2, i / 100f);
            //Vector2 tPointC2 = Vector2.Lerp(tPointB2, tPointB3, i / 100f);

            //Vector2 tResultPoint = Vector2.Lerp(tPointC1, tPointC2, i / 100f);
            track.Add(tResultPoint);

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
            Ball.transform.localPosition = Vector3.Lerp(track[i - 1], track[i], Time.deltaTime * 100);
            i++;
            if(i >= track.Count)
            {
                i = 1;
            }
        }

        //for (int j = 0; j < UPPER; j++)
        //{
            
        //}
    }
}
