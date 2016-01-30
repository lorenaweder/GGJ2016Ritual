using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//public enum Actions
//{
//	ATTACK,
//	DEFEND,
//	BUFF,
//	DEBUFF
//}
//
//public enum Elements
//{
//	FIRE,
//	WATER,
//	EARTH,
//	AIR
//}

public struct ActionType
{
	public Letters action {get; private set;}
	public Letters element {get; private set;}

	public ActionType(Letters action, Letters element)
	{
		this.action = action;
		this.element = element;
	}
}


public enum Buttons
{
	NONE,
	UP,
	DOWN,
	RIGHT,
	LEFT
}

public enum Letters
{
	NONE,
	ATTACK,
	DEFEND,
	BUFF,
	DEBUFF,
	FIRE,
	WATER,
	EARTH,
	AIR
}

//class ActionType : IEqualityComparer<ActionType>
//{
//	private Letters firstLetter;
//	private Letters secondLetter;
//
//	public ActionType(Letters l1, Letters l2) { firstLetter = l1; secondLetter = l2; }
//
//	bool IEqualityComparer<ActionType>.Equals(ActionType a1, ActionType a2)
//	{
//		if (a1 == null && a2 == null)
//			return true;
//		else if (a1 == null || a2 == null)
//			return false;
//		else
//			return a1.firstLetter.Equals(a2.firstLetter) && a1.secondLetter.Equals(a2.secondLetter);
//	}
//
//	int IEqualityComparer<ActionType>.GetHashCode(ActionType obj)
//	{
//		return obj.firstLetter.GetHashCode() ^ obj.secondLetter.GetHashCode();
//	}
//
//}


public class Game : MonoBehaviour {

	const int MAX_LETTERS = 2;
	Queue<Letters> word = new Queue<Letters>();

	Letters letterToQueue;
	Buttons buttonPressed = Buttons.NONE;

	int keysPressedCount = 0;
	KeyCode waitForUpCode;
	bool waitForUp = false;

	bool upPressed 		= false;
	bool downPressed 	= false;
	bool leftPressed 	= false;
	bool rightPressed 	= false;


	ActionType actionToExecute;

	Dictionary<int, int> availableActions = new Dictionary<int, int>(16);
//	ActionType fireAttack 	= new ActionType(Letters.ATTACK, Letters.FIRE);
//	ActionType fireDefend 	= new ActionType(Letters.DEFEND, Letters.FIRE);
//	ActionType waterAttack 	= new ActionType(Letters.ATTACK, Letters.WATER);
//	ActionType waterDefend 	= new ActionType(Letters.DEFEND, Letters.WATER);

	void Start()
	{
//		Debug.Log(fireAttack.GetHashCode());
//		availableActions.Add(fireAttack.GetHashCode(), 0);
//		availableActions.Add(fireDefend.GetHashCode(), 0);
//		availableActions.Add(waterAttack.GetHashCode(), 0);
//		availableActions.Add(waterDefend.GetHashCode(), 0);
		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.FIRE.GetHashCode(), 0);
	}

	void Update()
	{
		CheckInput();
		CheckWords();
	}


	void CheckWords()
	{
		if(word.Count < MAX_LETTERS)
		{
			buttonPressed = GetButton();
			if(buttonPressed == Buttons.NONE)
				return;

			letterToQueue = GetLetterForInput(buttonPressed, word.Count);
			if(letterToQueue == Letters.NONE)
				return;
			
			word.Enqueue(letterToQueue);
			Debug.Log("Agregado: " + letterToQueue);
		}
		else
		{
			int action = word.Dequeue().GetHashCode() + word.Dequeue().GetHashCode();
			int result;

			if(availableActions.TryGetValue(action, out result))
				Debug.Log("Debería ejecutarse: " + result);
			else
				Debug.Log("Option not available");
		}
	}

	Letters GetLetterForInput(Buttons button, int order)
	{
		switch(button)
		{
		case Buttons.NONE:
			return Letters.NONE;
		case Buttons.RIGHT:
			if(order == 0)
				return Letters.ATTACK;
			else
				return Letters.FIRE;
			break;
		case Buttons.LEFT:
			if(order == 0)
				return Letters.DEFEND;
			else
				return Letters.WATER;
			break;
		case Buttons.UP:
			if(order == 1)
				return Letters.AIR;
			break;
		case Buttons.DOWN:
			if(order == 1)
				return Letters.EARTH;
			break;
		}
		return Letters.NONE;
	}

	Buttons GetButton()
	{
		if(upPressed)
			return Buttons.UP;

		if(downPressed)
			return Buttons.DOWN;

		if(rightPressed)
			return Buttons.RIGHT;

		if(leftPressed)
			return Buttons.LEFT;

		return Buttons.NONE;
		
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

	void ExecuteAction(ActionType action)
	{
		
	}
}
