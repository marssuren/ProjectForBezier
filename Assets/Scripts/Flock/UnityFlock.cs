using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//对于单个物体来说，需要遵循3个原则
//1.分离——避免挤到鸟群中去 或与其他的成员发生碰撞
//2.队列——向鸟群的平均方向移动  飞行方向与附近的邻居的平均方向一致
//3.聚集——向鸟群的中心位置移动  不要落单
public class UnityFlock : MonoBehaviour     //boid是Craig Reynold创造的术语       用来描述群组中的每个个体对象
{
	public float minSpeed = 20f;            //最小移动速度
	public float turnSpeed = 20f;           //转向速度
	public float randomFreq = 20f;          //更新randomPush变量的频率
	public float randomForce = 20f;         //随机变化的力度（为了让移动的群组更真实）

    //seperation variables	分离的属性		参数用来让每个Boid个体保持安全距离。
    public float avoidanceRadius = 50f;     //规避力
    public float avoidanceForce = 20f;      //规避半径

    //alignment variables	队列的属性
    public float toOriginForce = 50f;       //向心力：用于保持所有的boid在一个范围内，并与群组的原点保持一定的距离。
	public float toOriginRange = 100f;      //向心区间：群组扩展的程度。
	public float gravity = 2f;              //吸引力
    

    //cohesion variables	凝聚的属性		参数用来让每个Boid个体与群组的领导者或群组的原点保持最小距离。
    public float followVelocity = 1f;       //追随速度
	public float followRadius = 5f;         //追随半径

	//these variables control the movement
	private Transform origin;               //设为父对象，以控制整个群组中的对象
    [SerializeField]
	private Vector3 velocity;               //最终的移动速度(带方向) 实时计算
	private Vector3 normalizedVelocity;     //归一化速度(带方向)
	private Vector3 randomPush;             //随机的推力：randomPush的值的更新基于randomForce的值，用于产生一个随机增长和降低的速度，使得群组的移动看上去更真实。
	private Vector3 originPush;             //父对象的推力
	private Transform[] objects;            //用于存储相邻boid的位置信息
	private UnityFlock[] otherFlocks;       //其他成员个体的集合
	private Transform transformComponent;


	void Start()
	{
		randomFreq = 1f;                //随机变化的频率
		origin = transform.parent;      //指定父物体为原点
		//Flock transform
		transformComponent = transform;
		//Temporary components
		Component[] tempFlocks = null;      
		//根据父物体获取所有Boid
		if(transform.parent)
		{
			tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
		}
        //获取所有相邻的Boid的数据信息
		objects = new Transform[tempFlocks.Length];
		otherFlocks = new UnityFlock[tempFlocks.Length];
        //加载群体的位置信息和群体到数组
		for(int i = 0; i < tempFlocks.Length; i++)
		{
			objects[i] = tempFlocks[i].transform;
			otherFlocks[i] = tempFlocks[i].GetComponent<UnityFlock>();

		}
		transform.parent = null;

	
        //根据设置好的随机频率更新速度
		StartCoroutine(UpdateRandom());
	}
	void Update()
	{ 
		//1.首先检查当前boid与其他boid之间的距离，并相应地更新速度
		float tSpeed = velocity.magnitude;  //速度的大小
		Vector3 tAvgVelocity = Vector3.zero;
		Vector3 tAvgPosition = Vector3.zero;
		float tCount = 0;
		float tF = 0f;
		float tDistance = 0f;
		Vector3 tMyPosition = transformComponent.position;
		Vector3 tForceV;
		Vector3 tToArg;
		Vector3 tWantedVel;
        #region 此处代码用于实现"分离"规则。首先，检查当前boid与其他boid之间的距离，并相应的更新速度，接下来，用当前速度除以群组中的boid的数目，计算出群组的平均速度
        for (int i = 0; i < objects.Length; i++)
        {
            Transform tTransform = objects[i];  //除我之外的成员的位置
            if (tTransform != transformComponent)
            {
                Vector3 tOtherPosition = tTransform.position;
                //平均速度 计算聚合
                tAvgPosition += tOtherPosition;
                tCount++;
                //其他群体到这个的向量
                tForceV = tMyPosition - tOtherPosition;
                //求向量长度(方向)
                tDistance = tForceV.magnitude;
                //向量长度小于追随半径，跟上
                if (tDistance < followRadius)
                {
                    //当前向量长度小于规避半径，基于逃离半径计算对象的速度
                    //如果两物体的当前距离已经小于最小距离，根据物体间的最小距离计算物体的速度
                    if (tDistance < avoidanceRadius)
                    {
                        tF = 1 - (tDistance / avoidanceRadius);
                        if (tDistance > 0)
                        {
                            tAvgVelocity += (tForceV / tDistance) * tF * avoidanceForce;    //求得自己的单位向量
                        }
                    }
                    //保持与头的距离
                    tF = tDistance / followRadius;
                    UnityFlock tOtherSealgull = otherFlocks[i];
                    //自己的速度与朝向取决于其他个体的速度与朝向 计算出后设置给自己
                    tAvgVelocity += tOtherSealgull.normalizedVelocity * tF * followVelocity;
                }
            }
        } 
        #endregion
        //2.用当前速度除以群组中的boid的数目，计算出群组的平均速度。
        if(tCount > 0)
		{
			//获得平均速度(队列)
			tAvgVelocity /= tCount;
			//获得平均位置与对象间的向量(凝聚)
			tToArg = (tAvgPosition / tCount) - tMyPosition;
		}
		else
		{
			tToArg = Vector3.zero;
		}
		
        //朝向领队
		tForceV = origin.position - tMyPosition;    //计算领队与我的位置的距离
		tDistance = tForceV.magnitude;              //距离的大小
		tF = tDistance / toOriginRange;             //计算大小是半径的多少
		//Calculate the velocity of the Flocks to the leader
		if(tDistance > 0)       //if this Boid is not at the center of the Flock    //不在群组中间的时候
		{
			originPush = (tForceV / tDistance) * toOriginForce;     //给一个初始的速度
		}

		if(tSpeed < minSpeed && tSpeed > 0)     //速度的值小于最小速度
		{
			velocity = (velocity / tSpeed) * minSpeed;  //速度就是最小速度  给最小速度改下方向
		}

		tWantedVel = velocity;
		//计算最终速度
		tWantedVel -= tWantedVel * Time.deltaTime;      
		tWantedVel += randomPush * Time.deltaTime;      //随机的推力
		tWantedVel += originPush * Time.deltaTime;      //初始的推力
		tWantedVel += tAvgVelocity * Time.deltaTime;    //其他成员的速度
		tWantedVel += tToArg.normalized * gravity * Time.deltaTime; //根据吸引力调节速度快慢
		//调整速度并使其转向最终速度
		velocity = Vector3.RotateTowards(velocity, tWantedVel, turnSpeed * Time.deltaTime, 100f);
		transformComponent.rotation = Quaternion.LookRotation(velocity);

		
        //根据计算后的速度移动对象
		transformComponent.Translate(velocity * Time.deltaTime, Space.World);
		//更新标准向量的引用
		normalizedVelocity = velocity.normalized;
	}
    //基于randomFreq的频率来更新randompush的频率
    IEnumerator UpdateRandom()
	{
		while(true)
		{
            //每隔一段时间返回半径为randomForce的球体内的随机一个点坐标，配合随机力度来更新randomPush
            randomPush = Random.insideUnitSphere * randomForce;
			yield return new WaitForSeconds(randomFreq * Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));//根据randomFreq在一定时间范围内
            //更新randomPush
        }
	}
}
