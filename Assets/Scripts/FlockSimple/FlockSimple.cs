using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSimple : MonoBehaviour
{
	internal FlockSimpleController flockController;
	internal Rigidbody _rigidbody;
	internal Rigidbody rigidbody
	{
		get
		{
			if(null == _rigidbody)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}

			return _rigidbody;
		}
	}

	void Update()
	{
		if(flockController)
		{
			Vector3 tRelativePos = steer() * Time.deltaTime;
			if(tRelativePos != Vector3.zero)
			{
				rigidbody.velocity = tRelativePos;
			}
			//enforce minimum and maximum speeds for the boids 
			float tSpeed = rigidbody.velocity.magnitude;
			if(tSpeed > flockController.maxVelocity)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * flockController.maxVelocity;
			}
			else if(tSpeed < flockController.minVelocity)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * flockController.minVelocity;
			}
		}
	}
	private Vector3 steer()
	{
		Vector3 tCenter = flockController.flockCenter - transform.localPosition;        //cohesion
		Vector3 tVelocity = flockController.flockVelocity - rigidbody.velocity;         //alignment
		Vector3 tFollow = flockController.Target.localPosition - transform.localPosition;       //follow leader
		Vector3 separation = Vector3.zero;
		for(int i = 0; i < flockController.flockList.Count; i++)
		{
			FlockSimple tFlockSimple = flockController.flockList[0];
			if(tFlockSimple != this)
			{
				Vector3 tRelativePos = transform.localPosition - tFlockSimple.transform.localPosition;
				separation += tRelativePos;         //(relativePos.sqrMagnitude)
			}
		}
		//randomize
		Vector3 tRandomize = new Vector3((Random.value * 2) - 1, (Random.value * 2 - 1), (Random.value * 2) - 1);
		tRandomize.Normalize();
		return (flockController.centerWeight * tCenter + flockController.velocityWeight * tVelocity + flockController.SeparationWeight * separation + flockController.followWeight * tFollow + flockController.randomizeWeight * tRandomize);
	}
}
