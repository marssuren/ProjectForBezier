using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpNumber : MonoBehaviour {

	int result = 0;
	private int start = 1;
	private int end = 10;
	private int jumpTimes = 9;

	private Text label = null;
	void Start()
	{
		label = gameObject.GetComponent<Text>();
		StartCoroutine(CorJumpNumber());
	}

	public IEnumerator CorJumpNumber()
	{
		int delta = (end - start) / jumpTimes;
		result = 0;

		for(int i = 0; i < jumpTimes; i++)
		{
			result += delta;
			label.text = result.ToString();
			yield return new WaitForSeconds(1);
		}

		result = end;
		label.text = result.ToString();
		StopCoroutine(CorJumpNumber());
	}
}
