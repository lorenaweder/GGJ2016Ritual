using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	int keysPressedCount = 0;
	KeyCode waitForUpCode;
	bool waitForUp = false;

	public KeyCode upKey 	= KeyCode.UpArrow;
	public KeyCode downKey 	= KeyCode.DownArrow;
	public KeyCode rightKey = KeyCode.RightArrow;
	public KeyCode leftKey 	= KeyCode.LeftArrow;

	public bool upPressed 	{get;private set;}
	public bool downPressed {get;private set;}
	public bool leftPressed {get;private set;}
	public bool rightPressed {get;private set;}

	void Awake()
	{
		SetAllInputsToFalse();
	}

	public void CheckInput()
	{
		keysPressedCount = 0;
		SetAllInputsToFalse();

		if(!waitForUp && Input.GetKeyDown(upKey))
		{
			upPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = upKey;
		}
		else if(waitForUp && Input.GetKeyUp(upKey) && waitForUpCode == upKey)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(downKey))
		{
			downPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = downKey;
		}
		else if(waitForUp && Input.GetKeyUp(downKey) && waitForUpCode == downKey)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(rightKey))
		{
			rightPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = rightKey;
		}
		else if(waitForUp && Input.GetKeyUp(rightKey) && waitForUpCode == rightKey)
		{
			waitForUp = false;
		}

		if(!waitForUp && Input.GetKeyDown(leftKey))
		{
			leftPressed = true;
			keysPressedCount++;

			waitForUp = true;
			waitForUpCode = leftKey;
		}
		else if(waitForUp && Input.GetKeyUp(leftKey) && waitForUpCode == leftKey)
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
		waitForUp = false;
	}
}