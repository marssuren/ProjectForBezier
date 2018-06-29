using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFollowing : MonoBehaviour
{
	[SerializeField]
	private Path path;
	public float Speed = 20f;
	public float Mass = 5f;
	public bool IsLooping = false;

	//Actual speed of the vehicle
	private float curSpeed;		//当前速度
	private int curPathIndex;	//当前处于第几个寻路点
	private float pathLength;	//路径点的长度
	private Vector3 targetPoint;	//当前目标点

	private Vector3 velocity;
	void Start()
	{
		pathLength = path.Length;
		curPathIndex = 0;
		//get the current velocity of the vehicle
		velocity = transform.forward;
	}
	void Update()
	{
		//Unify the speed
		curSpeed = Speed * Time.deltaTime;
		targetPoint = path.GetPoint(curPathIndex);
		//If reach the radius within the path then move to next point in the path
		if(Vector3.Distance(transform.position, targetPoint) < path.Radius)
		{
			//Don't move the vehicle if path is finished
			if(curPathIndex < pathLength - 1)
			{
				curPathIndex++;
			}
			else if(IsLooping)
			{
				curPathIndex = 0;
			}
			else
			{
				return;
			}
		}
		//Move the vehicle until the end point is reached in the path
		if(curPathIndex >= pathLength)
		{
			return;
		}
		//Calculate the next Velocity towards the path
		if(curPathIndex >= pathLength - 1 && !IsLooping)
		{
			velocity += Steer(targetPoint, true);
		}
		else
		{
			velocity += Steer(targetPoint);
		}
		//Move the vehicle according to the velocity 
		transform.position += velocity;
		//Rotate the vehicle towards desired Velocity
		transform.rotation = Quaternion.LookRotation(velocity);
	}
	public Vector3 Steer(Vector3 _target, bool _isFinalPoint = false)
	{
		//Calculate the directional vector from the current position towards the target point
		Vector3 tDesiredVelocity = (_target - transform.position);	//获得当前目标点距离
		float tDist = tDesiredVelocity.magnitude;	//算出长度
		//Normalize the desired Velocity
		tDesiredVelocity.Normalize();	//算出方向
		//Calculate the velocity according to the speed
		if(_isFinalPoint && tDist < 10f)			//到达终点
		{
			tDesiredVelocity *= (curSpeed * (tDist / 10f));	//减速
		}
		else
		{
			tDesiredVelocity *= curSpeed;
		}
		//Calculate the force vector
		Vector3 tSteeringForce = tDesiredVelocity - velocity;
		Vector3 tAcceleration = tSteeringForce / Mass;
		return tAcceleration;		//加速度
	}
}
