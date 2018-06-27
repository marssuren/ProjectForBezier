using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFlock : MonoBehaviour     //boid
{
	public float minSpeed = 20f;
	public float turnSpeed = 20f;
	public float randomFreq = 20f;          //更新randomPush变量的次数
	public float randomForce = 20f;

	//alignment variables	队列的属性
	public float toOriginForce = 50f;       //用于保持所有的boid在一个范围内，并与群组的原点保持一定的距离。
	public float toOriginRange = 100f;      //群组扩展的程度。


	public float gravity = 2f;
	//seperation variables	分离的规则		参数用来让每个Boid个体保持最小距离。
	public float avoidanceRadius = 50f;
	public float avoidanceForce = 20f;

	//cohesion variables	凝聚的规则		参数用来让每个Boid个体与群组的领导者或群组的原点保持最小距离。
	public float followVelocity = 4f;
	public float followRadius = 40f;

	//these variables control the movement
	private Transform origin;               //设为父对象，以控制整个群组中的对象
	private Vector3 velocity;
	private Vector3 normalizedVelocity;
	private Vector3 randomPush;             //randomPush的值的更新基于randomForce的值，用于产生一个随机增长和降低的速度，使得群组的移动看上去更真实。
	private Vector3 originPush;
	private Transform[] objects;            //用于存储相邻boid的信息
	private UnityFlock[] otherFlocks;       //用于存储相邻boid的信息
	private Transform transformComponent;


	void Start()
	{
		randomFreq = 1f;
		//Assign the parent as origin
		origin = transform.parent;
		//Flock transform
		transformComponent = transform;
		//Temporary components
		Component[] tempFlocks = null;
		//Get all the unity flock components from the parent
		//transform in the group
		if(transform.parent)
		{
			tempFlocks = transform.parent.GetComponentsInChildren<UnityFlock>();
		}
		//Assign and store all the flock objects in this group
		objects = new Transform[tempFlocks.Length];
		otherFlocks = new UnityFlock[tempFlocks.Length];
		for(int i = 0; i < tempFlocks.Length; i++)
		{
			objects[i] = tempFlocks[i].transform;
			otherFlocks[i] = tempFlocks[i].GetComponent<UnityFlock>();

		}
		//Null Parent as the flock leader will be UnityFlockController Object
		transform.parent = null;

		//Caculate random push depends on the random frequency provided
		StartCoroutine(UpdateRandom());
	}
	void Update()
	{
		//1.首先检查当前boid与其他boid之间的距离，并相应地更新速度
		//Internal variables
		float tSpeed = velocity.magnitude;
		Vector3 tAvgVelocity = Vector3.zero;
		Vector3 tAvgPosition = Vector3.zero;
		float tCount = 0;
		float tF = 0f;
		float tDistance = 0f;
		Vector3 tMyPosition = transformComponent.position;
		Vector3 tForceV;
		Vector3 tToArg;
		Vector3 tWantedVel;
		for(int i = 0; i < objects.Length; i++)
		{
			Transform tTransform = objects[i];
			if(tTransform != transformComponent)
			{
				Vector3 tOtherPosition = tTransform.position;
				//Average position to calculate cohesion
				tAvgPosition += tOtherPosition;
				tCount++;
				//Directional vector from other flock to this flock
				tForceV = tMyPosition - tOtherPosition;
				//Magnitude of that directional vector(Length)
				tDistance = tForceV.magnitude;
				//Add push value 1f the magnitude ,the length of the vector ,is less than followRadius to the leader
				if(tDistance < followRadius)
				{
					//calculate the velocity, the speed of the object,based on the avoidance distance between flocks if the current magnitude is less than the specified avoidance radius
					if(tDistance < avoidanceRadius)
					{
						tF = 1 - (tDistance / avoidanceRadius);
						if(tDistance > 0)
						{
							tAvgVelocity += (tForceV / tDistance) * tF * avoidanceForce;
						}
					}
					//just keep the current distance with the leader
					tF = tDistance / followRadius;
					UnityFlock tOtherSealgull = otherFlocks[i];
					//normalize the otherSealgull velocity vector to get the direction of movement, then we set a new velocity
					tAvgVelocity += tOtherSealgull.normalizedVelocity * tF * followVelocity;
				}
			}
		}
		//2.用当前速度除以群组中的boid的数目，计算出群组的平均速度。
		if(tCount > 0)
		{
			//Calculate the average flock velocity(Alignment)
			tAvgVelocity /= tCount;
			//Calculate Center value of the flock(Cohension)
			tToArg = (tAvgPosition / tCount) - tMyPosition;
		}
		else
		{
			tToArg = Vector3.zero;
		}
		//Directional Vector to the leader
		tForceV = origin.position - tMyPosition;
		tDistance = tForceV.magnitude;
		tF = tDistance / toOriginRange;
		//Calculate the velocity of the Flocks to the leader
		if(tDistance > 0)       //if this Boid is not at the center of the Flock
		{
			originPush = (tForceV / tDistance) * toOriginForce;
		}

		if(tSpeed < minSpeed && tSpeed > 0)
		{
			velocity = (velocity / tSpeed) * minSpeed;
		}

		tWantedVel = velocity;
		//Calculate final velocity
		tWantedVel -= tWantedVel * Time.deltaTime;
		tWantedVel += randomPush * Time.deltaTime;
		tWantedVel += originPush * Time.deltaTime;
		tWantedVel += tAvgVelocity * Time.deltaTime;
		tWantedVel += tToArg.normalized * gravity * Time.deltaTime;
		//Final Velocity to rotate the flock into 
		velocity = Vector3.RotateTowards(velocity, tWantedVel, turnSpeed * Time.deltaTime, 100f);
		transformComponent.rotation = Quaternion.LookRotation(velocity);

		//Move the flock based on the calculated velocity
		transformComponent.Translate(velocity * Time.deltaTime, Space.World);
		//normalise the velocity
		normalizedVelocity = velocity.normalized;
	}
	IEnumerator UpdateRandom()
	{
		while(true)
		{
			randomPush = Random.insideUnitSphere * randomForce;
			yield return new WaitForSeconds(randomFreq * Random.Range(-randomFreq / 2.0f, randomFreq / 2.0f));
		}
	}
}
