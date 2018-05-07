using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//像弹簧一样跟随目标物体
public class Trace : MonoBehaviour 
{
	public Transform Target;
	public float Smooth=5.0f;
	void Update()
	{
		transform.position=Vector3.Lerp(transform.position,Target.position,Time.deltaTime*Smooth);
	}

}
