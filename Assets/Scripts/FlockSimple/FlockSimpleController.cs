using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSimpleController : MonoBehaviour
{
	public float minVelocity = 1;       //Min Velocity
	public float maxVelocity = 8;       //Max Flock Speed
	public int flockSize = 20;          //Number of flocks in the group

	//How far the boids should stick to the center (the more weighy stick closer to the center)
	public float centerWeight = 1;

	public float velocityWeight = 1;            //Alignment behavior


	//How far each boid should follow to the leader
	public float SeparationWeight = 1;
	//How close each boid should follow to the leader (the more weight make the closer follow)
	public float followWeight = 1;

	//Additional Random Noise
	public float randomizeWeight = 1;

	public FlockSimple Prefab;
	public Transform Target;

	//Center position of the flock in the group
	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;         //Average Velocity

	public List<FlockSimple> flockList = new List<FlockSimple>();
	void Start()
	{
		for(int i = 0; i < flockSize; i++)
		{
			FlockSimple tFlockSimple = Instantiate(Prefab, transform.position, transform.rotation);
			tFlockSimple.transform.parent = transform;
			tFlockSimple.flockController = this;
			flockList.Add(tFlockSimple);
		}
	}
	void Update()
	{
		//Calculate the Center and Velocity of the whole flock group
		Vector3 tCenter = Vector3.zero;
		Vector3 tVelocity = Vector3.zero;
		for(int i = 0; i < flockList.Count; i++)
		{
			FlockSimple tFlockSimple = flockList[i];
			tCenter += tFlockSimple.transform.localPosition;
			tVelocity += tFlockSimple.rigidbody.velocity;
		}

		flockCenter = tCenter / flockSize;
		flockVelocity = tVelocity / flockSize;
	}



}
