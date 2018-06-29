using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAvoidance : MonoBehaviour
{

	public float Speed = 20f;
	public float Mass = 5f;
	public float Force = 50f;
	public float minimumDistToAvoid = 20f;
	//Actual speed of the vehicle
	private float curSpeed;
	private Vector3 targetPoint;
	//Use this for initialization
	void Start()
	{
		Mass = 5f;
		targetPoint = Vector3.zero;
	}
	void OnGUI()
	{
		GUILayout.Label("Click anywhere to move the vehicle.");
	}
	void Update()
	{
		//Vehicle move by mouse click
		RaycastHit tHit;
		var tRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButtonDown(0) && Physics.Raycast(tRay, out tHit, 100f))
		{
			targetPoint = tHit.point;
		}
		//Directional vector to the target position
		Vector3 tDir = (targetPoint - transform.position);
		tDir.Normalize();
		//Apply obstacle avoidance
		avoidObstacles(ref tDir);

		//Don't move the vehicle when the target point is reached
		if(Vector3.Distance(targetPoint, transform.position) < 3f)
		{
			return;
		}
		//Assign the speed with delta time
		curSpeed = Speed * Time.deltaTime;
		//Rotate the vehicle to its target directional vector
		Quaternion tRotation = Quaternion.LookRotation(tDir);
		transform.rotation = Quaternion.Slerp(transform.rotation, tRotation, 5f * Time.deltaTime);
		//Move the vehicle towards
		transform.position += transform.forward * curSpeed;
	}
	private void avoidObstacles(ref Vector3 _dir)
	{
		RaycastHit tHit;
		//Only detect layer 9 (Obstacles)
		int tLayerMask = 1 << 9;
		//Check that the vehicle hit with the obstacles within it's minimum distance to avoid 
		if(Physics.Raycast(transform.position, transform.forward, out tHit, minimumDistToAvoid, tLayerMask))
		{
			//Get the normal of the hit point to calculate the new direction
			Vector3 tHitNormal = tHit.normal;
			tHitNormal.y = 0f;      //Don't want to move in Y-Space
									//Get the new directional vector by adding force to vehicle's current forward vector
			_dir = transform.forward + tHitNormal * Force;
		}
	}
}
