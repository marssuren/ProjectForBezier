using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//领导的作用是更新自己的位置，这样其他的个体就知道往哪走  在个体的origin中被引用
public class UnityFlockController : MonoBehaviour
{
	public Vector3 Offset;
	public Vector3 bound;
	public float Speed = 100f;
	private Vector3 initialPosition;
	private Vector3 nextMovementPoint;

	void Start()
	{
		initialPosition = transform.position;
		calculateNextMovementPoint();
	}
	void Update()   //自己动(此处转圈)
	{
		transform.Translate(Vector3.forward * Speed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation,
			Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);
		if(Vector3.Distance(nextMovementPoint, transform.position) <= 10f)
		{
			calculateNextMovementPoint();
		}
	}
	private void calculateNextMovementPoint()
	{
		float tPosX = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
		float tPosY = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
		float tPosZ = Random.Range(initialPosition.z - bound.y, initialPosition.y + bound.z);
		nextMovementPoint = initialPosition + new Vector3(tPosX, tPosY, tPosZ);
	}
}

