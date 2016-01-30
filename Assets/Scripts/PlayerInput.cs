using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	int keysPressedCount = 0;
	KeyCode waitForUpCode;
	bool waitForUp = false;

	public bool upPressed 	{get;private set;}
	public bool downPressed {get;private set;}
	public bool leftPressed {get;private set;}
	public bool rightPressed {get;private set;}

	void Awake()
	{
		SetAllInputsToFalse();
	}

	void CheckInput()
	{
		keysPressedCount = 0;
		SetAllInputsToFalse();

		if(!waitForUp && Input.GetKeyDown(KeyCode.UpArrow))
		{
			upPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = KeyCode.UpArrow;
		}
		else if(waitForUp && Input.GetKeyUp(KeyCode.UpArrow) && waitForUpCode == KeyCode.UpArrow)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(KeyCode.DownArrow))
		{
			downPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = KeyCode.DownArrow;
		}
		else if(waitForUp && Input.GetKeyUp(KeyCode.DownArrow) && waitForUpCode == KeyCode.DownArrow)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(KeyCode.RightArrow))
		{
			rightPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = KeyCode.RightArrow;
		}
		else if(waitForUp && Input.GetKeyUp(KeyCode.RightArrow) && waitForUpCode == KeyCode.RightArrow)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(KeyCode.LeftArrow))
		{
			leftPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = KeyCode.LeftArrow;
		}
		else if(waitForUp && Input.GetKeyUp(KeyCode.LeftArrow) && waitForUpCode == KeyCode.LeftArrow)
		{
			waitForUp = false;
		}


		if(keysPressedCount > 1)
		{
			SetAllInputsToFalse();
		}

	}

	void SetAllInputsToFalse()
	{
		upPressed = false;
		downPressed = false;
		rightPressed = false;
		leftPressed = false;
	}
}