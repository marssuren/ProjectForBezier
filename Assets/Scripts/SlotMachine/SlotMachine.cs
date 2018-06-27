using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GUIText))]
public class SlotMachine : MonoBehaviour
{
	public float SpinDuration = 2.0f;
	public int NumberOfSystem = 10;
	private GameObject BetResult;
	private bool startSpin;
	private bool firstReelSpinned;
	private bool secondReelSpinned;
	private bool thirdReelSpinned;

	private string betAmount = "100";

	private int firstReelResult = 0;
	private int secondReelResult = 0;
	private int thirdReelResult = 0;

	private float elapsedTime = 0.0f;
	void Start()
	{
		BetResult = gameObject;
		BetResult.GetComponent<GUIText>().text = "";
	}
	void OnGUI()
	{
		GUI.Label(new Rect(200, 40, 100, 20), "Your bet:");
		betAmount = GUI.TextField(new Rect(280, 40, 50, 20), betAmount, 25);
		if(GUI.Button(new Rect(Screen.width/2, Screen.height/2, 150, 40), "Pull Liver"))
		{
			Start();
			startSpin = true;
		}
	}
	void checkBet()
	{
		if(firstReelResult == secondReelResult && secondReelResult == thirdReelResult)
		{
			BetResult.GetComponent<GUIText>().text = "YOU WIN!!";
		}
		else
		{
			BetResult.GetComponent<GUIText>().text = "YOU LOSE!!";
		}
	}
	void FixedUpdate()
	{
		if(startSpin)
		{
			elapsedTime += Time.deltaTime;
			int tRandomSpinResult = Random.Range(0, NumberOfSystem);
			if(!firstReelSpinned)
			{
				GameObject.Find("firstReel").GetComponent<GUIText>().text = tRandomSpinResult.ToString();
				if(elapsedTime >= SpinDuration)
				{
					firstReelResult = tRandomSpinResult;
					firstReelSpinned = true;
					elapsedTime = 0;
				}
			}
			else if(!secondReelSpinned)
			{
				GameObject.Find("secondReel").GetComponent<GUIText>().text = tRandomSpinResult.ToString();
				if(elapsedTime >= SpinDuration)
				{
					secondReelResult = tRandomSpinResult;
					secondReelSpinned = true;
					elapsedTime = 0;
				}
			}
			else if(!thirdReelSpinned)
			{
				GameObject.Find("thirdReel").GetComponent<GUIText>().text = tRandomSpinResult.ToString();
				if(elapsedTime >= SpinDuration)
				{
					thirdReelResult = tRandomSpinResult;
					startSpin = false;
					elapsedTime = 0;
					firstReelSpinned = false;
					secondReelSpinned = false;
					checkBet();
				}
			}
		}
	}

}
